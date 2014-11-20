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
    internal partial class Server
    {
        private static object lockObject = new object();    // For when thread safety is needed

        private static byte[] _buffer = new byte[1024];
        private static volatile List<Client> _clientList = new List<Client>();
        private static Socket _serverSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int _PORT = 777;

        private static volatile CrusadeGame _Game;


        static void Main(string[] args)
        {
            Console.Title = "Crusade Server";
            SetupServer();

            Thread checkConnectionsThread = new Thread(PollClients);
            checkConnectionsThread.Start();

            Console.ReadLine();
        }


        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
            _serverSocket.Listen(2);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);

            Console.WriteLine("Server EndPoint: " + _serverSocket.LocalEndPoint.ToString());
            Console.WriteLine("Now accepting client connections.");
        }


        private static void PollClients()
        {
            while (true)
            {
                foreach (Client client in _clientList.ToArray())
                {
                    if (!isConnected(client))
                        DisconnectClient(client);

                    if (_clientList.Count < 2 && _Game != null)
                    {
                        ShutdownGame();

                        // Start accepting client connections again
                        _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);
                    }
                }
                Thread.Sleep(10);
            }
        }
       

        private static void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            Client newClient = new Client(socket);
            _clientList.Add(newClient);

            newClient.clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, 
                new AsyncCallback(ReceiveCallBack), newClient.clientSocket);


            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss: ") + "A client has connected");
            Console.WriteLine("Total clients connected: " + _clientList.Count + Environment.NewLine);

            if (_clientList.Count > 1)
            {
                BeginNewGame();
            }
            
            else // wait for more clients
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);

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

                if(received > 0 && isConnected(client))
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
                WriteErrorToConsole(error);
                WriteToErrorLog(error);

                _clientList.Remove(client);
                PrintNumConnections();
            }
        }
        

        private static bool isConnected(Client client)
        {
            try
            {
                return (client.clientSocket.Poll(-1, SelectMode.SelectRead) && client.clientSocket.Poll(-1, SelectMode.SelectWrite));             
            }
            catch (SocketException ex)
            {
                WriteErrorToConsole(ex.Message);
                WriteToErrorLog(ex.Message);
                return false;
            }
        } 


        private static void ProcessRequest(byte[] dataBuf, Client client)
        {
            if (dataBuf.Length < 1)
                return;

            RequestResponse.RequestType requestType = (RequestResponse.RequestType)dataBuf[0];

            switch(requestType)
            {
                case RequestResponse.RequestType.ClientRequest:
                    ProcessClientRequest(dataBuf, client);
                    break;

                case RequestResponse.RequestType.GameRequest:
                    ProcessGameRequest(dataBuf, client);
                    break;

                case RequestResponse.RequestType.MessageRequest:
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
            SendData(client, buffer, RequestResponse.ResponseType.MessageResponse);
        }


        private static void ProcessMessageRequest(byte[] dataBuf)
        {
            // strip requesttype byte
            Array.Copy(dataBuf, 1, dataBuf, 0, dataBuf.Length - 1);
            dataBuf[dataBuf.Length - 1] = (byte)'\0';

            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Broadcasting: " + Encoding.ASCII.GetString(dataBuf));

            // Broadcast Message
            for(int i = 0; i < _clientList.Count; ++i)
            {
                SendData(_clientList[i], dataBuf, RequestResponse.ResponseType.MessageResponse);
            }
        }


        private static void ProcessGameRequest(byte[] dataBuf, Client client)
        {
            string request = Encoding.ASCII.GetString(dataBuf, 1, dataBuf.Length - 1);
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "GAME REQ: " + request);

           // if (request == "GETGAMEBOARD")
                GiveClientsBoardState();
        }


        private static void ProcessClientRequest(byte[] dataBuf, Client client)
        {
            string clientMsg = Encoding.ASCII.GetString(dataBuf);

            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss CLIENT REQ: ") + clientMsg);
        }


        private static void SendData(Client client, byte[] bufferToSend, RequestResponse.ResponseType responseType)
        {
            if (!isConnected(client))
                return;

            try
            {
                byte[] buffer = new byte[bufferToSend.Length + 1];
                buffer[0] = (byte)responseType;
                Array.Copy(bufferToSend, 0, buffer, 1, bufferToSend.Length);

                if(isConnected(client))
                    client.clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
                        new AsyncCallback(SendCallBack), client.clientSocket);
            }
            catch (SocketException ex)
            {
                string error = "SendData Error: " + ex.Message;
                WriteErrorToConsole(error);
                WriteToErrorLog(error);
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
                WriteErrorToConsole(error);
                WriteToErrorLog(error);
            }
        }
        

        private static Client GetMatchingClient(Socket socket)
        {
            return _clientList.Find(a => a.clientSocket == socket);
        }


        private static void DisconnectClient(Client client)
        {
            Console.WriteLine("Disconnecting Client...");            
            client.clientSocket.Shutdown(SocketShutdown.Both);
            client.clientSocket.Disconnect(true);
            client.clientSocket.Close();
            _clientList.Remove(client);
        }
        
        
        private static void WriteToErrorLog(string error)
        {
            lock (lockObject)
            {
                string separator = Environment.NewLine + "============================" + Environment.NewLine;
                File.AppendAllText(DateTime.Now.ToString("yyyy-MM-dd ") + "Server Error Log.txt", 
                    separator + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss ") + error + separator);
            }
        }


        private static void WriteErrorToConsole(string error)
        {
            string separator = Environment.NewLine + "============================" + Environment.NewLine;
            Console.WriteLine(separator + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss ") + error + separator);
        }


        private static void PrintNumConnections()
        {
            Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + ": " + "Clients connected: "
                    + _clientList.Count + Environment.NewLine);
        }
   

    }
}
