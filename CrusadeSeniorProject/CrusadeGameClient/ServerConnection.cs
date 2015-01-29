using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using ReqRspLib;

namespace CrusadeGameClient
{
    internal class ServerConnection : ReqRspLib.ICrusadeClient
    {
        #region Fields

        private const int _port = 777;
        private readonly TcpClient _client;

        private BinaryFormatter binaryFormatter;

        private bool _shouldReceive = false;
        private bool _inAGame = false;
        private bool _isTurnPlayer = false;

        private Guid _clientId;
        private object _lockObject = new object();

        private string[,] _gameboard;

        #endregion


        #region Properties

        public Guid ID { get { return _clientId; } }

        public bool IsTurnPlayer
        {
            get { return _isTurnPlayer; }
            set { _isTurnPlayer = value; }
        }

        public List<ReqRspLib.ICard> Hand { get; set; }


        #endregion


        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public ServerConnection()
        {
            try
            {
                binaryFormatter = new BinaryFormatter();
               // IPAddress[] hosts = Dns.GetHostAddresses("primefusion.ddns.net");
                IPAddress[] hosts = Dns.GetHostAddresses("127.0.0.1");
                IPAddress serverIP = hosts[0];
                IPEndPoint ep = new IPEndPoint(serverIP, _port);

                _client = new TcpClient();
                _client.SendTimeout = 3000;
                _client.ReceiveTimeout = 3000;
                _client.Connect(ep);

                _shouldReceive = true;
                ThreadPool.QueueUserWorkItem(Receive);

                Hand = new List<ReqRspLib.ICard>();
                _gameboard = new string[1,1];
            }
            catch(SocketException ex)
            {
                WriteErrorToLog("Connect Error: " + ex.Message);
                WriteErrorToConsole("Connect Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            try
            {
                lock (_lockObject)
                {
                    _shouldReceive = false;
                    _client.Close();
                }
            }
            catch (SocketException ex)
            {
                WriteErrorToConsole("Disconnect Error: " + ex.Message);
                WriteErrorToLog("Disconnect Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Send a request to the server for the contents of the player's hand
        /// </summary>
        public void RequestGameHand()
        {
            ReqRspLib.RequestHand req = new RequestHand(_clientId);
            SendRequestToServer(req);
        }


        /// <summary>
        /// Request the state of the gameboard from the server.
        /// </summary>
        public void RequestGameboard()
        {
            ReqRspLib.RequestGameboard rsp = new RequestGameboard(ID);
            SendRequestToServer(rsp);
        }

        /// <summary>
        /// Updates the client's stored gameboard.
        /// </summary>
        /// <param name="newBoard">New gameboard state.</param>
        public void SetGameboard(string[,] newBoard)
        {
            lock(_gameboard)
                _gameboard = newBoard;
        }


        /// <summary>
        /// Display the contents of the player's hand on the console.
        /// </summary>
        public void DisplayHand()
        {
            lock (Hand)
            {
                Console.WriteLine(Environment.NewLine + "Hand:");
                for (int i = 0; i < Hand.Count; ++i)
                    Console.WriteLine("{0}: {1}", (i + 1).ToString(), Hand[i].Name);

                Console.WriteLine(Environment.NewLine);
            }
        }


        /// <summary>
        /// Displays the state of the gameboard on the console.
        /// </summary>
        public void DisplayGameboard()
        {
            Console.WriteLine("Gameboard State:" + Environment.NewLine);

            lock (_gameboard)
            {
                foreach (string item in _gameboard)
                {
                    Console.Write(item);
                }
            }
        }


        public void GetPlayerDecision()
        {
            Console.WriteLine("Select a card to play.");
            DisplayHand();
            int option = -1;
            bool validChoice = false;

            while(!validChoice)
            {
                option = Convert.ToInt32(Console.ReadKey().KeyChar) - 48;

                if ((option - 1) < Hand.Count && (option - 1) > -1)
                {
                    validChoice = true;
                    RequestPlayCard rsp = new RequestPlayCard(ID, (option - 1));
                    SendRequestToServer(rsp);
                }
                else
                    Console.WriteLine("Invalid Option\n");
            }

        }


        public void BeginGame()
        {
            if(!_inAGame)
            {
                _inAGame = true;
                RequestGameHand();
            }

        }


        public void EndGame()
        {
            _inAGame = false;
        }


        public void BeginNextTurn()
        {
            DisplayGameboard();

            if (_isTurnPlayer)
                GetPlayerDecision();

            else
                DisplayHand();
        }
        #endregion


        #region Private Methods

        private bool isConnected()
        {
            try
            {
                bool part1 = _client.Client.Poll(1000, SelectMode.SelectRead);
                bool part2 = (_client.Client.Available == 0);
                if ((part1 && part2) || !_client.Client.Connected)
                    return false;
                else
                    return true;
            }
            catch (SocketException ex)
            {
                WriteError("Client Connection Error: " + ex.Message);
                return false;
            }
            catch (NullReferenceException ex)
            {
                WriteError("Client Connection Error: " + ex.Message);
                return false;
            }
            catch (ObjectDisposedException ex)
            {
                WriteError("Client Connection Error: " + ex.Message);
                return false;
            }
        }

        private void SendRequestToServer(ReqRspLib.IRequest request)
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                binaryFormatter.Serialize(stream, request);
            }
            catch(SocketException ex)
            {
                WriteErrorToConsole(ex.Message);
                WriteErrorToLog(ex.Message);
            }
            catch(System.Runtime.Serialization.SerializationException ex)
            {
                WriteErrorToConsole("Send Error: " + ex.Message);
                WriteErrorToLog("Send Error: " + ex.Message);
            }
        }


        private void Receive(object obj)
        {
            using(NetworkStream stream = _client.GetStream())
            {
                while(_shouldReceive)
                {
                    byte[] length = new byte[2]; // Will hold how big the anticipated response is

                    try
                    {
                        int read = stream.Read(length, 0, 2);

                        if (read > 0) // Essentially we'll keep looping 'til something's been read
                        {
                            byte[] buffer = new byte[BitConverter.ToInt16(length, 0)];  // Find out how big the incoming response is
                            read = stream.Read(buffer, 0, buffer.Length);               // Read in the response

                            if (read > 0)
                            {
                                using(MemoryStream ms = new MemoryStream()) // A bit of a work around to be able to deserialize the buffer
                                {
                                    ms.Write(buffer, 0, buffer.Length);
                                    ms.Position = 0;                    // Need to be at the beginning of the stream before deserializing

                                    IResponse rsp = (IResponse)binaryFormatter.Deserialize(ms);
                                    ProcessResponse(rsp);
                                }
                            }
                        }
                    }
                    catch(SocketException ex)
                    {
                        WriteError("Receive socket Error: " + ex.Message);
                    }
                    catch(IOException ex)
                    {
                        if(!isConnected())
                            WriteError("Receive IO Error: " + ex.Message);
                    }
                    catch(System.Runtime.Serialization.SerializationException ex)
                    {
                        WriteError("Receive Serialize Error: " + ex.Message);
                    }
                }
            }            

            Console.WriteLine("No longer receiving messages.");
        }


        private void ProcessResponse(IResponse serverResponse)
        {
            if(serverResponse is ResponseClientID)
            {
                ResponseClientID rsp = serverResponse as ResponseClientID;
                _clientId = rsp.ID;

                Console.WriteLine("ID assigned: {0}", rsp.ID.ToString());
                Console.Title = "Client " + ID.ToString();
            }
         
            else
                serverResponse.Execute(this);
        }


        /// <summary>
        /// Writes the input error to both the console and a log file.
        /// </summary>
        /// <param name="error">Error to write.</param>
        private void WriteError(string error)
        {
            WriteErrorToLog(error);
            WriteErrorToConsole(error);
        }
        
        private void WriteErrorToLog(string error)
        {
            lock (_lockObject)
            {
                string path = DateTime.Now.ToString("yyyy-MM-dd") + " Client " + ID.ToString("N");
                string msg = DateTime.Now.ToString("hh:mm:ss ") + error + Environment.NewLine;
                File.AppendAllText(path, msg);
            }
        }

        private void WriteErrorToConsole(string error)
        {
            lock (_lockObject)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("====================================================================");
                Console.WriteLine(error);
                Console.WriteLine("====================================================================");
                Console.WriteLine(Environment.NewLine);
            }
        }

        #endregion
    }
}
