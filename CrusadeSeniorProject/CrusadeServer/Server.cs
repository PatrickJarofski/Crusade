using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace CrusadeServer
{
    public partial class Server
    {
        private const int _Port = 777;
        private bool shouldListen = true;

        private readonly TcpListener _tcpListener;
        private List<GameClient> _clientList;

        private BinaryFormatter binaryFormatter;
        private object lockObject = new object();

        private CrusadeLibrary.CrusadeGame _game;

        private Thread listenThread;
        private Thread pollThread;


        internal Server()
        {
            binaryFormatter = new BinaryFormatter();
            _clientList = new List<GameClient>();

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, _Port);
            _tcpListener = new TcpListener(ep);
            _tcpListener.Start();

            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();

            pollThread = new Thread(new ThreadStart(PollClients));
            pollThread.Start();

            Console.WriteLine("Now listening for clients on port {0}.", _Port.ToString());
        }


        private void PollClients()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            while(true)
            {
                sw.Restart();
                foreach(GameClient client in _clientList.ToArray())
                {
                    if(!isConnected(client))
                        DisconnectClient(client);
                }
                while (sw.ElapsedMilliseconds < 300) ;
            }
        }


        private bool isConnected(GameClient client)
        {
            try
            {
                bool part1 = client.TCPclient.Client.Poll(1000, SelectMode.SelectRead);
                bool part2 = (client.TCPclient.Available == 0);
                if ((part1 && part2) || !client.TCPclient.Connected)
                    return false;
                else
                    return true;
            }
            catch (SocketException ex)
            {
                WriteErrorToConsole("Client Connected Error: " + ex.Message);
                WriteErrorToLog("Client Connected Error: " + ex.Message);
                return false;
            }
            catch (NullReferenceException ex)
            {
                WriteErrorToConsole("Client Connected Error: " + ex.Message);
                WriteErrorToLog("Client Connected Error: " + ex.Message);
                return false;
            }
            catch(ObjectDisposedException ex)
            {
                WriteErrorToConsole("Client Connected Error: " + ex.Message);
                WriteErrorToLog("Client Connected Error: " + ex.Message);
                return false;
            }
        }


        private void DisconnectClient(GameClient client)
        {
            try
            {                
                client.PlayerNumber = CrusadeLibrary.Player.PlayerNumber.NotAPlayer;

                lock(_clientList)
                    _clientList.Remove(client);

                Console.WriteLine("Client disconnected. {0}", client.TCPclient.Client.RemoteEndPoint.ToString());
                client.TCPclient.Close();                
            }
            catch(NullReferenceException ex)
            {
                WriteErrorToConsole("Disconnect Client Error: " + ex.Message);
                WriteErrorToLog("Disconnect Client Error: " + ex.Message);
            }
        }
 

        private async void ListenForClients()
        {
            while(shouldListen)
            {
                try
                {
                    if (_clientList.Count < 2)
                    {
                        TcpClient client = await _tcpListener.AcceptTcpClientAsync();

                        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                        clientThread.Start(client);

                        if(_clientList.Count == 2)
                        {
                            _game = new CrusadeLibrary.CrusadeGame();
                            _clientList[0].PlayerNumber = CrusadeLibrary.Player.PlayerNumber.PlayerOne;
                            _clientList[1].PlayerNumber = CrusadeLibrary.Player.PlayerNumber.PlayerTwo;

                            Console.WriteLine("Game started.");
                        }
                    }
                }
                catch (SocketException ex)
                {
                    WriteErrorToLog("Client Listen Error: " + ex.Message);
                    WriteErrorToConsole("Client Listen Error: " + ex.Message);
                }
            }
        }


        private void HandleClientComm(object obj)
        {
            GameClient newClient = new GameClient((TcpClient)obj, Guid.NewGuid());

            lock(_clientList)
                _clientList.Add(newClient);

            NetworkStream clientStream = newClient.TCPclient.GetStream();
            IPEndPoint ep = (IPEndPoint)newClient.TCPclient.Client.RemoteEndPoint;
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Client connected. " + ep.ToString());
            Console.WriteLine("ID assigned: {0}", newClient.ID.ToString());

            SendClientId(newClient);

            while(isConnected(newClient))
            {
                try
                {
                    IRequest request = (IRequest)binaryFormatter.Deserialize(newClient.TCPclient.GetStream());
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRequest), request);
                }
                catch(System.Runtime.Serialization.SerializationException ex)
                {
                    WriteErrorToConsole(ex.Message);
                    WriteErrorToLog(ex.Message);
                }
            }
            
        }


        private void SendClientId(GameClient newClient)
        {
            ResponseClientID rsp = new ResponseClientID(newClient.ID);
            SendData(newClient, rsp);
        }


        private void ProcessRequest(object state)
        {
            IRequest request = (IRequest)state;
            request.Execute(this);
        }


        /// <summary>
        /// Gets the GameClient that matches the given Guid.
        /// </summary>
        /// <param name="ep">Guid to match</param>
        /// <returns>GameClient whose Guid matches the given Guid.</returns>
        public GameClient GetMatchingClient(Guid id)
        {
            Console.WriteLine("Given id: " + id.ToString());
            foreach(GameClient client in _clientList)
            {
                if (client.ID == id)
                    return client;
            }

            throw new NullReferenceException("The given ID does not match any GameClients.");
        }


        private void SendData(GameClient client, IResponse rsp)
        {
            try
            {
                NetworkStream stream = client.TCPclient.GetStream();
                binaryFormatter.Serialize(stream, rsp);
            }
            catch(SocketException ex)
            {
                WriteErrorToConsole("Send Data Error: " + ex.Message);
                WriteErrorToLog("Send Data Error: " + ex.Message);
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                WriteErrorToConsole("Send Data Error: " + ex.Message);
                WriteErrorToLog("Send Data Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Writes the given string to the server's error log.
        /// </summary>
        /// <param name="error">String to write</param>
        internal void WriteErrorToLog(string error)
        {
            lock (lockObject)
            {
                string path = DateTime.Now.ToString("YYYY-MM-DD") + " Client Log";
                string msg = DateTime.Now.ToString("hh:mm:ss ") + error;
                System.IO.File.AppendAllText(path, msg);
            }
        }

        /// <summary>
        /// Writes a given string to the console with additional formatting for readability.
        /// </summary>
        /// <param name="error">String to write.</param>
        internal void WriteErrorToConsole(string error)
        {
            lock (lockObject)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("====================================================================");
                Console.WriteLine(error);
                Console.WriteLine("====================================================================");
                Console.WriteLine(Environment.NewLine);
            }
        }


    }
}
