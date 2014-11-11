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
        private static List<Client> _clientSockets = new List<Client>();
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
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);
            Console.WriteLine("Now accepting client connections.");
        }
        

        private static bool isConnected(Client client)
        {
            try
            {
                return !(client.clientSocket.Poll(1, SelectMode.SelectRead) && client.clientSocket.Available == 0);
            }
            catch(SocketException)
            {
                _clientSockets.Remove(client);
                client.clientSocket.Disconnect(true);
                client.clientSocket.Close();
                return false;
            }
        }


        private static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            Client newClient = new Client(socket);
            _clientSockets.Add(newClient);

            if(_clientSockets.Count == 2)
                BeginNewGame();

            newClient.clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), newClient.clientSocket);

            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);

            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss: ") + "A client has connected");
            Console.WriteLine("Total clients connected: " + _clientSockets.Count + Environment.NewLine);
        }


        private static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            Client client = GetMatchingClient(socket);

            if (!isConnected(client))
                return;

            try
            {
                int received = client.clientSocket.EndReceive(ar);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);

                // Check what type of request we've got
                // and deal with it accordingly
                if(received > 0)
                    ProcessRequest(dataBuf, client);

                if (isConnected(client))
                    client.clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, 
                        new AsyncCallback(ReceiveCallBack), client.clientSocket);  
                
                else
                    PrintNumConnections();                
            } 

            catch (SocketException ex)
            {
                string error = "ReceiveCallback Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);

                _clientSockets.Remove(client);
                PrintNumConnections();
            }
        }


        private static void ProcessRequest(byte[] dataBuf, Client client)
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


        private static void ProcessBadRequest(Client client)
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


        private static void ProcessGameRequest(byte[] dataBuf, Client client)
        {
            string request = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "GAME REQ: " + request);

            // TODO: Figure out the rest
        }


        private static void ProcessClientRequest(byte[] dataBuf, Client client)
        {
            string clientMsg = Encoding.ASCII.GetString(dataBuf);

            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss CLIENT REQ: ") + clientMsg);

            if (clientMsg == "CLOSE_CONNECTION")
            {
                string broadcastMsg = Environment.NewLine + "A client has disconnected." + Environment.NewLine;
                DisconnectClient(client);

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


        private static void SendData(Client client, byte[] bufferToSend)
        {
            try
            {
                client.clientSocket.BeginSend(bufferToSend, 0, bufferToSend.Length, SocketFlags.None,
                    new AsyncCallback(SendCallBack), client.clientSocket);
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
                Client client = GetMatchingClient((Socket)ar.AsyncState);
                client.clientSocket.EndSend(ar);
            }
            catch(SocketException ex)
            {
                string error = "SendCallback Error: " + ex.Message;
                WriteErrorToConsole(ref error);
                WriteToErrorLog(ref error);
            }
        }
        

        private static Client GetMatchingClient(Socket socket)
        {
            return _clientSockets.Find(a => a.clientSocket == socket);
        }


        private static void DisconnectClient(Client client)
        {
            _clientSockets.Remove(client);
            client.clientSocket.Disconnect(true);
            client.clientSocket.Close();
        }
        

        private static void BeginNewGame()
        {
            Random rng = new Random();
            int playerOne = rng.Next(0, 2);

            if (playerOne == 1)
            {
                _clientSockets[0].PlayerID = Client.PlayerNumber.PlayerOne;
                _clientSockets[1].PlayerID = Client.PlayerNumber.PlayerTwo;
            }
            else
            {
                _clientSockets[1].PlayerID = Client.PlayerNumber.PlayerOne;
                _clientSockets[0].PlayerID = Client.PlayerNumber.PlayerTwo;
            }

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
