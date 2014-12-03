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

        private readonly string _name;

        private object lockObject = new object();

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public bool Connected
        {
            get 
            {
                try
                {
                    bool write = _clientSocket.Poll(10, SelectMode.SelectWrite);
                    bool read = _clientSocket.Poll(10, SelectMode.SelectRead);
                    bool connected = _clientSocket.Connected;

                    return (write && connected);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + ex.Message + Environment.NewLine);
                    return false;
                }
            }
        }


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
                _clientSocket.BeginSend(request, 0, request.Length,
                        SocketFlags.None, new AsyncCallback(SendCallback), _clientSocket);
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
                CrusadeServer.StateObject state = new CrusadeServer.StateObject();
                state.workerSocket = _clientSocket;

                _clientSocket.BeginReceive(state.buffer, 0, CrusadeServer.StateObject.BufferSize,
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
                CrusadeServer.StateObject state = (CrusadeServer.StateObject)ar.AsyncState;
                Socket client = state.workerSocket;

                int bytesReceived = client.EndReceive(ar);

                if (bytesReceived > 0)
                {
                    response = Encoding.ASCII.GetString(state.buffer, 0, bytesReceived);
                    response = response.TrimEnd('\0');

                    if (response.Length > 0)
                    {
                        // In case we get several responses
                        // concatenated together
                        string[] responses = response.Split(CrusadeServer.Constants.Delimiters, StringSplitOptions.RemoveEmptyEntries);
                    
                        foreach (string rsp in responses)
                        {
                            JSONResponse jsonResponse = JSONResponse.ConvertToJson(rsp);
                            _GameClient.UpdateFromServer(jsonResponse.responseType, jsonResponse.response);
                        }
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
            Console.WriteLine("Sent request to server: " + request);
        }


        public void SendMessageRequest(string request)
        {
            SendRequestToServer(GetBuffer(RequestTypes.MessageRequest, request));
        }


        private byte[] GetBuffer(byte requestType, string request)
        {
            JSONRequest jsonRequest = new JSONRequest();

            IPEndPoint ep = (IPEndPoint)_clientSocket.LocalEndPoint;
            jsonRequest.RequestIP = ep.Address.ToString();
            jsonRequest.RequestPort = ep.Port;
            jsonRequest.RequestType = requestType;
            jsonRequest.Request = request;

            Console.WriteLine("Sending request: " + JSONRequest.ConvertToString(jsonRequest));

            return Encoding.ASCII.GetBytes(JSONRequest.ConvertToString(jsonRequest) + CrusadeServer.Constants.ResponseDelimiter.ToString());
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
    }
}
