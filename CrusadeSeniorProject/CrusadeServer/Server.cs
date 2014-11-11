using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using CrusadeLibrary;
using System.Threading;

namespace CrusadeServer
{
    class Program
    {
        private static byte[] _buffer = new byte[1024];
        private static List<Socket> _clientSockets = new List<Socket>();
        private static Socket _serverSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int _PORT = 777;

        private static CrusadeGame _Game;

        static void Main(string[] args)
        {
            Console.Title = "Crusade Server";
            SetupServer();
            Console.ReadLine();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            _serverSocket.Listen(2);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            Console.WriteLine("Now accepting client connections.");
        }
        

        private static bool isConnected(Socket client)
        {
            try
            {
                return !(client.Poll(1, SelectMode.SelectRead) && client.Available == 0);
            }
            catch(SocketException)
            {
                _clientSockets.Remove(client);
                client.Disconnect(true);
                client.Close();
                return false;
            }
        }



        private static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            _clientSockets.Add(socket);

            if(_clientSockets.Count == 2)
            {
                BeginNewGame();
            }

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);

            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss: ") + "A client has connected");
            Console.WriteLine("Total clients connected: " + _clientSockets.Count + Environment.NewLine);
        }


        private static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            if (!isConnected(socket))
                return;

            try
            {
                int received = socket.EndReceive(ar);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);

                // Check what type of request we've got
                // and deal with it accordingly
                ProcessRequest(dataBuf, socket);

                if (socket.Connected)
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);  
                
                else
                    PrintNumConnections();                
            } 

            catch (SocketException ex)
            {
                string error = "ReceiveCallback Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);

                _clientSockets.Remove(socket);
                PrintNumConnections();
            }
        }


        private static void ProcessRequest(byte[] dataBuf, Socket client)
        {
            if (dataBuf.Length < 1)
                return;

            byte requestType = dataBuf[0];

            switch(requestType)
            {
                case RequestType.Client_Request:
                    ProcessClientRequest(dataBuf, client);
                    break;

                case RequestType.Game_Request:
                    ProcessGameRequest(dataBuf, client);
                    break;

                case RequestType.Message_Request:
                    ProcessMessageRequest(dataBuf);
                    break;

                default:
                    ProcessBadRequest(client);
                    break;
            }
        }


        private static void ProcessBadRequest(Socket client)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Bad Request");

            byte[] buffer = Encoding.ASCII.GetBytes("Bad Request");
            SendData(client, buffer);
        }


        private static void ProcessMessageRequest(byte[] dataBuf)
        {
            // Strip requestType byte
            byte[] message = StripTypeByte(dataBuf);

            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Broadcasting: " + Encoding.ASCII.GetString(message));

            // Broadcast Message
            for(int i = 0; i < _clientSockets.Count; ++i)
            {
                SendData(_clientSockets[i], message);
            }
        }


        private static void ProcessGameRequest(byte[] dataBuf, Socket client)
        {
            string request = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "GAME REQ: " + request);

            // TODO: Figure out the rest
        }


        private static void ProcessClientRequest(byte[] dataBuf, Socket client)
        {
            string clientMsg = Encoding.ASCII.GetString(dataBuf);

            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss CLIENT REQ: ") + clientMsg);

            if (clientMsg == "CLOSE_CONNECTION")
            {
                string broadcastMsg = Environment.NewLine + "A client has disconnected." + Environment.NewLine;
                _clientSockets.Remove(client);
                client.Disconnect(true);
                client.Close();

                byte[] buffer = Encoding.ASCII.GetBytes("1" + broadcastMsg);
                ProcessMessageRequest(buffer);
            }
        }


        private static byte[] StripTypeByte(byte[] array)
        {
            byte[] returnArray = new byte[array.Length - 1];
            Array.Copy(array, 1, returnArray, 0, (array.Length - 1));

            return returnArray;
        }


        private static void SendData(Socket socket, byte[] bufferToSend)
        {
            try
            {
                socket.BeginSend(bufferToSend, 0, bufferToSend.Length, SocketFlags.None,
                    new AsyncCallback(SendCallBack), socket);
            }
            catch (SocketException ex)
            {
                string error = "SendData Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);
            }
        }


        private static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndSend(ar);
            }
            catch(SocketException ex)
            {
                string error = "SendCallback Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);
            }
        }
        

        private static void BeginNewGame()
        {
            _Game = new CrusadeGame();
        }


        private static void WriteToErrorLog(ref string error)
        {
            string separator = Environment.NewLine + "============================" + Environment.NewLine;
            File.AppendAllText("Server Error Log.txt", separator + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss ") + error + separator);
        }


        private static void WriteErrorToConsole(ref string error)
        {
            string separator = Environment.NewLine + "============================" + Environment.NewLine;
            Console.WriteLine(separator + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss ") + error + separator);
        }


        private static void PrintNumConnections()
        {
            Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + ": " + "Clients connected: "
                    + _clientSockets.Count + Environment.NewLine);
        }
   

    }
}
