using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using ReqRspLib;
using System.Text;

namespace CrusadeServer
{
    public partial class Server : ReqRspLib.ICrusadeServer
    {
        private const int _Port = 777;
        private bool shouldListen = true;

        private readonly TcpListener _tcpListener;
        private List<GameClient> _clientList;

        private BinaryFormatter binaryFormatter;
        private object lockObject = new object();

        private CrusadeLibrary.CrusadeGame _game = null;

        private int ClientsConnected = 0;


        internal Server()
        {
            Console.Title = "Server";

            binaryFormatter = new BinaryFormatter();
            _clientList = new List<GameClient>();

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, _Port);
            _tcpListener = new TcpListener(ep);
            _tcpListener.Start();

            ThreadPool.QueueUserWorkItem(ListenForClients);
           // ThreadPool.QueueUserWorkItem(PollClients);

            Console.WriteLine("Now listening for clients on port {0}.", _Port.ToString());
        }


        private void PollClients(object obj)
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
                --ClientsConnected;
            }
            catch(NullReferenceException ex)
            {
                WriteErrorToConsole("Disconnect Client Error: " + ex.Message);
                WriteErrorToLog("Disconnect Client Error: " + ex.Message);
            }
        }
 

        private async void ListenForClients(object obj)
        {
            while(shouldListen)
            {
                try
                {
                    if (_clientList.Count < 2)
                    {
                        TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                        ++ClientsConnected;                   

                        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                        clientThread.Start(client); 
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

            Console.WriteLine(Environment.NewLine + DateTime.Now.ToString("hh:mm:ss ") + "Client connected. " + ep.ToString());
            Console.WriteLine("ID assigned: {0}", newClient.ID.ToString());

            PrintNumberOfClients();
            SendClientId(newClient);
            CheckIfNewGame();           

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
                    DisconnectClient(newClient);
                }
                catch(System.IO.IOException ex)
                {
                    WriteErrorToConsole(ex.Message);
                    WriteErrorToLog(ex.Message);
                    DisconnectClient(newClient);
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


        private void SendData(Guid clientId, IResponse rsp)
        {
            SendData(GetMatchingClient(clientId), rsp);
        }

        
        private void SendData(GameClient client, IResponse rsp)
        {
            if (client == null || rsp == null)
                return;

            try
            {
                using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    binaryFormatter.Serialize(ms, rsp);
                    byte[] rspBytes = ms.ToArray();                         // Get the bytes of the response
                    short length = (short)rspBytes.Length;                  // Get how long the response is

                    byte[] buffer = new byte[length + 2];                   // Have two extra bytes since the length will be prepended
                    Array.Copy(BitConverter.GetBytes(length), buffer, 2);   // Put the length in the first two bytes
                    Array.Copy(rspBytes, 0, buffer, 2, length);             // Put the response after the length
                    
                    NetworkStream stream = client.TCPclient.GetStream();    // Get the stream
                    stream.Write(buffer, 0, buffer.Length);                 // Ship the object off
                }
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


        private void BroadcastToClients(IResponse rsp)
        {
            foreach (GameClient client in _clientList.ToArray())
                SendData(client, rsp);
        }


        private void CheckIfNewGame()
        {
            int count;
            lock (_clientList)
                count = _clientList.Count;

            if (count == 2)
            {
                lock (lockObject)
                    _game = new CrusadeLibrary.CrusadeGame();

                int num = CrusadeLibrary.CrusadeGame.RNG.Next() % 1;

                if(num == 0)
                {
                    _clientList[0].isTurnPlayer = true;
                    _clientList[0].PlayerNumber = CrusadeLibrary.Player.PlayerNumber.PlayerOne;
                    _clientList[1].isTurnPlayer = false;
                    _clientList[1].PlayerNumber = CrusadeLibrary.Player.PlayerNumber.PlayerTwo;
                }
                else
                {
                    _clientList[1].isTurnPlayer = true;
                    _clientList[1].PlayerNumber = CrusadeLibrary.Player.PlayerNumber.PlayerOne;
                    _clientList[0].isTurnPlayer = false;
                    _clientList[0].PlayerNumber = CrusadeLibrary.Player.PlayerNumber.PlayerTwo;
                }

                Console.WriteLine("Game started.");
                BroadcastToClients(new ResponseStartGame());

                foreach (GameClient client in _clientList)
                {
                    GivePlayerHand(client.ID);
                    GivePlayerGameboard(client.ID);
                }

                BeginNextTurn();
            }
        }


        private void PrintNumberOfClients()
        {
            lock(_clientList)
                Console.WriteLine(Environment.NewLine + "Number of clients: {0}" + Environment.NewLine, _clientList.Count.ToString());
        }
    }
}
