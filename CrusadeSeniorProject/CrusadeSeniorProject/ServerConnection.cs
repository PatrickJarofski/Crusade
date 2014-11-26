using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

using CrusadeServer;

namespace CrusadeSeniorProject
{
    public class ServerConnection
    {
        private readonly CrusadeGameClient _GameClient;

        private readonly Socket _clientSocket;
        private const int _Port = 777;

        private int idCount = 0;
        private readonly string _name;

        private object lockObject = new object();

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public ServerConnection(CrusadeGameClient game)
        {
            _GameClient = game;
            _name = "SomeName";

            try
            {
                _clientSocket = new Socket
                    (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress[] ipHostInfo = Dns.GetHostAddresses("primefusion.ddns.net");
                IPAddress _IPAddress = ipHostInfo[0];

                IPAddress DebugAddress = IPAddress.Parse("127.0.0.1");

                IPEndPoint endpoint = new IPEndPoint(_IPAddress, _Port);

                _clientSocket.ReceiveTimeout = 3000;
                _clientSocket.SendTimeout = 3000;

                _clientSocket.Connect(endpoint);
                Receive();

            }

            catch(SocketException ex)
            {
                WriteToErrorLog("ACTION: Constructor " + ex.Message);
                throw new Exception(ex.Message);
            }
            
            SendMessageRequest("New Client " + _name + " has joined.");
        }


        private void SendRequestToServer(byte[] request)
        {
            try
            {
                string debug = Encoding.ASCII.GetString(request);
                LogRequest(debug);

                _clientSocket.BeginSend(request, 0, request.Length,
                        SocketFlags.None, new AsyncCallback(SendCallback), null);
            }

            catch (SocketException ex)
            {
                WriteToErrorLog("ACTION: SendMessage() " + ex.Message);
            }

            catch (ObjectDisposedException ex)
            {
                WriteToErrorLog("ACTION: SendMessage() " + ex.Message);
            }
            
            sendDone.WaitOne();
            sendDone.Reset();
        }


        private void SendCallback(IAsyncResult ar)
        {
            try
            {               
                int bytesSend = _clientSocket.EndSend(ar);
                sendDone.Set();
            }
            catch(SocketException ex)
            {
                throw ex;
            }
        }


        private void Receive()
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = _clientSocket;

                _clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize,
                        SocketFlags.None, new AsyncCallback(ReceiveCallback), state);
            }

            catch(SocketException ex)
            {
                WriteToErrorLog("ACTION: Receive() " + ex.Message);
            }
        }


        private void ReceiveCallback(IAsyncResult ar)
        {
            string response = string.Empty;
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesReceived = client.EndReceive(ar);

                if (bytesReceived > 0)
                {
                    state.stringBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesReceived));

                    response = state.stringBuilder.ToString();
                    response = response.TrimEnd('\0');

                    if (response.Length > 0)
                    {
                        JSONResponse jsonResponse = JSONResponse.ConvertToJson(response);
                        _GameClient.UpdateFromServer(jsonResponse.responseType, jsonResponse.response);
                    }
                }

                // Begin a another async receive
                Receive();
            }

            catch (SocketException ex)
            {
                WriteToErrorLog(ex.Message);
                response = ex.Message;
            }

            catch (ObjectDisposedException ex)
            {
                WriteToErrorLog(ex.Message);
                response = ex.Message;
            }
        }       


        public void SendClientRequest(string request)
        {
            SendRequestToServer(GetBuffer(RequestTypes.ClientRequest, request));
        }


        public void SendGameRequest(string request)
        {
            SendRequestToServer(GetBuffer(RequestTypes.GameRequest, request));
        }


        public void SendMessageRequest(string request)
        {
            SendRequestToServer(GetBuffer(RequestTypes.MessageRequest, request));
        }


        private byte[] GetBuffer(string requestType, string request)
        {
            JSONRequest jsonRequest = new JSONRequest();

            jsonRequest.ID = ++idCount;

            IPEndPoint ep = (IPEndPoint)_clientSocket.LocalEndPoint;
            jsonRequest.requestIP = ep.Address.ToString();
            jsonRequest.requestPort = ep.Port;
            jsonRequest.requestType = requestType;
            jsonRequest.request = request;

            return Encoding.ASCII.GetBytes(JSONRequest.ConvertToString(jsonRequest));
        }
        

        public void RequestBoardState()
        {
            SendGameRequest("GETGAMEBOARD");
        }


        public void EndConnection()
        {
            try
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
            }

            catch (SocketException ex)
            {
                WriteToErrorLog(ex.Message);
            }
        }


        public void WriteToErrorLog(string msg)
        {
            lock (lockObject)
            {
                File.AppendAllText("Client Error Log.txt", Environment.NewLine + 
                    DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss: ") + msg + Environment.NewLine);
            }
        }


        private void LogRequest(string request)
        {
            lock(lockObject)
                File.AppendAllText("Client Request Log.txt", request + Environment.NewLine);
        }
    }
}
