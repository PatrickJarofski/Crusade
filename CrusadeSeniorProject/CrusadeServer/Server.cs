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
        private object lockObject = new object();    // For when thread safety is needed

        private volatile List<Client> _clientList = new List<Client>();
        private Socket _serverSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int _PORT = 777;

        private volatile CrusadeGame _Game;
        public volatile bool KeepPollingClients = true;

        static void Main(string[] args)
        {
            Console.Title = "Crusade Server";
            Server server = new Server();

            Console.WriteLine("\n\nTo shutdown the server press Enter.");
            Console.Read();

            server.KeepPollingClients = false;
        }


        public Server()
        {
            try
            {
                Console.WriteLine("Setting up server...");

                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _PORT));
                _serverSocket.Listen(2);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);

                Console.WriteLine("Server EndPoint: " + _serverSocket.LocalEndPoint.ToString());
                Console.WriteLine("Now accepting client connections.");

                Thread checkConnectionsThread = new Thread(PollClients);
                checkConnectionsThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }


        private void PollClients()
        {
            while (KeepPollingClients)
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
       

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Client newClient = new Client(listener.EndAccept(ar));
            _clientList.Add(newClient);

            StateObject state = new StateObject();
            state.workerSocket = newClient.clientSocket;

            newClient.clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None, 
                new AsyncCallback(ReceiveCallBack), state);

            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss: ") + "A client has connected");
            Console.WriteLine("Total clients connected: " + _clientList.Count + Environment.NewLine);

            if (_clientList.Count > 1)
                BeginNewGame();
            
            else // wait for more clients
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), _serverSocket);

        }


        private void ReceiveCallBack(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Client client = GetMatchingClient(state.workerSocket);

            if (!isConnected(client))
                return;

            try
            {
                int received = client.clientSocket.EndReceive(ar);
  
                // Check what type of request we've got
                // and deal with it accordingly
                if (isConnected(client))
                {
                    if(received > 0)
                        ProcessRequest(state.buffer, client);

                    state.Clear();
                    client.clientSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None,
                        new AsyncCallback(ReceiveCallBack), state);  
                }
                
                else
                    PrintNumConnections();                
            } 

            catch (SocketException ex)
            {
                string error = "ReceiveCallback Error: " + ex.Message;
                WriteErrorToConsole(error);
                WriteErrorToLog(error);

                _clientList.Remove(client);
                PrintNumConnections();
            }
        }
        

        private bool isConnected(Client client)
        {
            try
            {
                return (client.clientSocket.Poll(-1, SelectMode.SelectRead) && client.clientSocket.Poll(-1, SelectMode.SelectWrite));             
            }
            catch (SocketException ex)
            {
                WriteErrorToConsole(ex.Message);
                WriteErrorToLog(ex.Message);
                Console.WriteLine(Environment.NewLine + ex.Message + Environment.NewLine);
                return false;
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(Environment.NewLine + "ERROR: " + ex.Message + Environment.NewLine);
                return false;
            }
        } 


        private void ProcessRequest(byte[] dataBuf, Client client)
        {
            if (dataBuf.Length < 1)
                return;

            JSONRequest request = JSONRequest.ConvertToJson(Encoding.ASCII.GetString(dataBuf).Trim('\0'));
            Console.WriteLine("Receive request: " + request.Request);

            switch(request.RequestType)
            {
                case RequestTypes.ClientRequest:
                    ProcessClientRequest(request);
                    break;

                case RequestTypes.GameRequest:
                    ProcessGameRequest(request);
                    break;

                case RequestTypes.MessageRequest:
                    ProcessMessageRequest(request);
                    break;

                default:
                    ProcessBadRequest(request);
                    break;
            }
        }


        private void ProcessBadRequest(JSONRequest request)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Bad Request");

            SendData(GetMatchingClient(request.RequestIP, request.RequestPort), 
                GenerateResponse(ResponseTypes.MessageResponse, "BAD REQUEST"));
        }


        private void ProcessMessageRequest(JSONRequest jsonRequest)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Broadcasting: " + jsonRequest.Request);

            // Broadcast Message
            foreach(Client client in _clientList)
            {
                SendData(client, GenerateResponse(ResponseTypes.MessageResponse, jsonRequest.Request));
            }
        }


        private void ProcessGameRequest(JSONRequest jsonRequest)
        {            
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "GAME REQ: " + jsonRequest.Request);

            if (jsonRequest.Request == CrusadeServer.Requests.GetGameboard)
                GiveClientsBoardState();

            if (jsonRequest.Request == CrusadeServer.Requests.GetPlayerhand)
            {
                Client client = GetMatchingClient(jsonRequest.RequestIP, jsonRequest.RequestPort);
                SendPlayerHand(client.PlayerID);
            }
        }


        private void ProcessClientRequest(JSONRequest jsonRequest)
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss CLIENT REQ: ") + jsonRequest.Request);
        }


        private void SendData(Client client, JSONResponse jsonResponse)
        {
            if (!isConnected(client))
                return;

            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(JSONResponse.ConvertToString(jsonResponse) + Constants.ResponseDelimiter);

                if(isConnected(client))
                    client.clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
                        new AsyncCallback(SendCallBack), client.clientSocket);
            }
            catch (SocketException ex)
            {
                string error = "SendData Error: " + ex.Message;
                WriteErrorToConsole(error);
                WriteErrorToLog(error);
            }
        }


        private void SendCallBack(IAsyncResult ar)
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
                WriteErrorToLog(error);
            }
        }
        

        private JSONResponse GenerateResponse(byte responseType, string response)
        {
            JSONResponse jsonResponse = new JSONResponse();
            jsonResponse.responseType = responseType;
            jsonResponse.response = response;

            return jsonResponse;
        }


        private Client GetMatchingClient(string ip, int port)
        {
            if (ip == string.Empty)
                return null;

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);

            foreach(Client client in _clientList)
            {
                if (client.clientSocket.LocalEndPoint == ep)
                    return client;
            }

            // Unable to locate client
            // Something strange has happened
            throw new Exception("A request was received from an unconnected client.");
        }


        private Client GetMatchingClient(Socket socket)
        {
            return _clientList.Find(a => a.clientSocket == socket);
        }


        private Client GetMatchingClient(Player.PlayerNumber player)
        {
            foreach (Client client in _clientList)
                if (client.PlayerID == player)
                    return client;

            throw new Exception("The specified player does not exist.");
        }


        private void DisconnectClient(Client client)
        {
            Console.WriteLine("Disconnecting Client...");            
            client.clientSocket.Shutdown(SocketShutdown.Both);
            client.clientSocket.Disconnect(true);
            client.clientSocket.Close();
            _clientList.Remove(client);
        }
        
        
        private void WriteErrorToLog(string error)
        {
            lock (lockObject)
            {
                string separator = Environment.NewLine + "============================" + Environment.NewLine;
                File.AppendAllText(DateTime.Now.ToString("yyyy-MM-dd ") + "Server Error Log.txt", 
                    separator + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss ") + error + separator);
            }
        }


        private void WriteErrorToConsole(string error)
        {
            string separator = Environment.NewLine + "============================" + Environment.NewLine;
            Console.WriteLine(separator + DateTime.Now.ToString("yyyy/MM/dd||hh:mm:ss ") + error + separator);
        }


        private void PrintNumConnections()
        {
            Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + ": " + "Clients connected: "
                    + _clientList.Count + Environment.NewLine);
        }   

    }
}
