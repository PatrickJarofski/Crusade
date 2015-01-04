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
        }


        private void DisconnectClient(GameClient client)
        {
            try
            {
                client.PlayerNumber = CrusadeLibrary.Player.PlayerNumber.NotAPlayer;

                lock(_clientList)
                    _clientList.Remove(client);

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
                        }
                    }
                }
                catch (SocketException ex)
                {
                    WriteErrorToLog(ex.Message);
                    Console.WriteLine("ListenForClient() error written to log.");
                }
            }
        }


        private void HandleClientComm(object obj)
        {
            GameClient newClient = new GameClient((TcpClient)obj);

            lock(_clientList)
                _clientList.Add(newClient);

            NetworkStream clientStream = newClient.TCPclient.GetStream();
            IPEndPoint ep = (IPEndPoint)newClient.TCPclient.Client.RemoteEndPoint;
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss ") + "Client connected. " + ep.ToString());

            while(isConnected(newClient))
            {
                try
                {
                    IRequest request = (IRequest)binaryFormatter.Deserialize(newClient.TCPclient.GetStream());
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRequest), request);

                    ResponseTest rsp = new ResponseTest();
                    binaryFormatter.Serialize(clientStream, rsp);
                }
                catch(System.Runtime.Serialization.SerializationException ex)
                {
                    WriteErrorToConsole(ex.Message);
                    WriteErrorToLog(ex.Message);
                }
            }
            
        }


        private void ProcessRequest(object state)
        {
            IRequest request = (IRequest)state;
            request.Execute(this);
        }


        private GameClient GetMatchingClient(TcpClient tcpClient)
        {
            foreach(GameClient client in _clientList)
            {
                if (client.TCPclient == tcpClient)
                    return client;
            }

            return null;
        }

        

        private void WriteErrorToLog(string error)
        {
            lock (lockObject)
            {
                string path = DateTime.Now.ToString("YYYY-MM-DD") + " Client Log";
                string msg = DateTime.Now.ToString("hh:mm:ss ") + error;
                System.IO.File.AppendAllText(path, msg);
            }
        }


        private void WriteErrorToConsole(string error)
        {
            lock (lockObject)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("==================================");
                Console.WriteLine(error);
                Console.WriteLine("==================================");
                Console.WriteLine(Environment.NewLine);
            }
        }


    }
}
