using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

using RequestType = CrusadeServer.RequestResponse.RequestType;
using ResponseType = CrusadeServer.RequestResponse.ResponseType;

namespace CrusadeSeniorProject
{
    public class ServerConnection
    {
        private readonly CrusadeGameClient _GameClient;

        private readonly Socket _clientSocket;
        const int _Port = 777;

        private readonly string _name;

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


                //_clientSocket.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), _clientSocket);
                //connectDone.WaitOne();
            }

            catch(SocketException ex)
            {
                WriteToErrorLog("ACTION: Constructor " + ex.Message);
            }
            
            SendMessageRequest("New Client " + _name + " has joined.");
        }


        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                connectDone.Set();
            }
            catch(SocketException ex)
            {
                WriteToErrorLog("ACTION: ConnectCallback() " + ex.Message);
            }
        }


        private void SendRequestToServer(byte[] request)
        {
            try
            {
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

            Receive();
            receiveDone.WaitOne();
            receiveDone.Reset();
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

                state.stringBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesReceived));

                response = state.stringBuilder.ToString();
                response = response.TrimEnd('\0');

                if(response.Length > 0)
                {
                    ResponseType responseType = (ResponseType)state.buffer[0];
                    string msg = response.Substring(1);
                    _GameClient.UpdateFromServer(responseType, msg);
                }
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
            finally
            {
                receiveDone.Set();
            }
        }       


        public void SendClientRequest(string request)
        {
            byte[] buffer = formatRequest(request, (byte)RequestType.ClientRequest);
            SendRequestToServer(buffer);
        }


        public void SendGameRequest(string request)
        {
            byte[] buffer = formatRequest(request, (byte)RequestType.GameRequest);
            SendRequestToServer(buffer);
        }


        public void SendMessageRequest(string request)
        {
            byte[] buffer = formatRequest(request, (byte)RequestType.MessageRequest);
            SendRequestToServer(buffer);
        }


        private byte[] formatRequest(string request, byte requestType)
        {
            byte[] buffer = new byte[request.Length + 1];
            buffer[0] = requestType;

            Encoding.ASCII.GetBytes(request, 0, request.Length, buffer, 1);
            return buffer;
        }
        

        public void RequestBoardState()
        {
            SendGameRequest("GETGAMEBOARD");
        }


        public void EndConnection()
        {
            try
            {
              //  SendClientRequest("CLOSE_CONNECTION");

                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
            }

            catch (SocketException ex)
            {
                WriteToErrorLog(ex.Message);
            }
        }


        public static void WriteToErrorLog(string msg)
        {
            File.AppendAllText("Client Error Log.txt", Environment.NewLine + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss: ") + msg + Environment.NewLine);
        }
    }
}
