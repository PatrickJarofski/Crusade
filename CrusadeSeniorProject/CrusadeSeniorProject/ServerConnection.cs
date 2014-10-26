using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

namespace CrusadeSeniorProject
{
    public class ServerConnection
    {
        private readonly CrusadeGameClient _GameClient;

        readonly Socket _clientSocket;
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
                IPEndPoint endpoint = new IPEndPoint(_IPAddress, _Port);

                //_clientSocket.Connect(endpoint);

                _clientSocket.ReceiveTimeout = 3000;
                _clientSocket.SendTimeout = 3000;
                _clientSocket.BeginConnect(endpoint, new AsyncCallback(ConnectCallback), _clientSocket);
                connectDone.WaitOne();
            }

            catch(SocketException ex)
            {
                WriteToErrorLog("ACTION: Constructor " + ex.Message);
            }
            
            SendMessage("New Client " + _name + " has joined.");
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


        public void SendMessage(string message)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(message);

                _clientSocket.BeginSend(byteData, 0, byteData.Length,
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
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesReceived = _clientSocket.EndReceive(ar);

                // There might be more data, so store the data received so far.
                state.stringBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesReceived));

                //client.BeginReceive(state.buffer, 0, StateObject.BufferSize,
                //    SocketFlags.None, new AsyncCallback(ReceiveCallback), state);          

                if(state.stringBuilder.Length > 1)
                {
                    string msg = state.stringBuilder.ToString();
                    msg = msg.TrimEnd('\0');

                    if (msg != "CLOSE_CONNECTION")
                        _GameClient.UpdateFromServer(msg);
                }                                     
            }

            catch (SocketException ex)
            {
                throw ex;
            }

            catch (ObjectDisposedException ex)
            {
                WriteToErrorLog(ex.Message);
            }

            receiveDone.Set(); 
        }       


        internal void EndConnection()
        {
            try
            {
                SendMessage("CLOSE_CONNECTION");

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
            File.AppendAllText("Errorlog.txt", Environment.NewLine + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss: ") + msg + Environment.NewLine);
        }
    }
}
