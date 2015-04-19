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

        private ReqRspLib.ClientGamePiece[,] _gameboard;

        private static ServerConnection _instance;

        private List<ReqRspLib.ClientCard> _hand;

        private bool handChanged = false;
        private bool boardChanged = false;

        #endregion


        #region Properties

        public Guid ID { get { return _clientId; } }

        public int BackRow { get; set; }

        public bool IsTurnPlayer
        {
            get { return _isTurnPlayer; }
            set { _isTurnPlayer = value; }
        }

        public bool InAGame { get { return _inAGame; } }

        public static ServerConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServerConnection();

                return _instance;
            }
        }

        public List<ReqRspLib.ClientCard> Hand { get { return _hand; } }

        public ReqRspLib.ClientGamePiece[,] Gameboard { get { return _gameboard; } }

        public bool HandUpdated 
        { 
            get
            {   // don't want to keep grabbing a new hand if it hasn't changed since last check
                if(handChanged) 
                {
                    handChanged = false;
                    return true;
                }
                return handChanged; 
            } 
        }

        public bool BoardUpdated 
        {
            get
            {   // don't want to keep grabbing a new board if it hasn't changed since last check
                if(boardChanged) 
                {
                    boardChanged = false;
                    return true;
                }
                return boardChanged; 
            } 
        }

        #endregion

        #region ClientInput

        public void PlayCard(int index, int row, int col)
        {
            ReqRspLib.RequestPlayCard req = new RequestPlayCard(ID, index, row, col);
            SendRequestToServer(req);
        }

        public void MoveTroop(GameCell source, GameCell dest)
        {
            RequestMoveTroop req = new RequestMoveTroop(ID, source.Row, source.Col, dest.Row, dest.Col);
            SendRequestToServer(req);
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        private ServerConnection()
        {
            try
            {
                binaryFormatter = new BinaryFormatter();
                 IPAddress[] hosts = Dns.GetHostAddresses("primefusion.ddns.net");
                //IPAddress[] hosts = Dns.GetHostAddresses("127.0.0.1");
                IPAddress serverIP = hosts[0];
                IPEndPoint ep = new IPEndPoint(serverIP, _port);

                _client = new TcpClient();
                _client.SendTimeout = 3000;
               // _client.ReceiveTimeout = 3000;
                _client.Connect(ep);

                _shouldReceive = true;
                ThreadPool.QueueUserWorkItem(Receive);

                _hand = new List<ReqRspLib.ClientCard>();
                _gameboard = new ClientGamePiece[1, 1];
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
            ReqRspLib.RequestGameboard rsp = new ReqRspLib.RequestGameboard(ID);
            SendRequestToServer(rsp);
        }


        /// <summary>
        /// Set the client's hand with a new/updated one.
        /// </summary>
        /// <param name="newHand">List of cards in the hand.</param>
        public void SetHand(List<ClientCard> newHand)
        {
            _hand = newHand;
            handChanged = true;
        }


        /// <summary>
        /// Updates the client's stored gameboard.
        /// </summary>
        /// <param name="newBoard">New gameboard state.</param>
        public void SetGameboard(ClientGamePiece[,] newBoard)
        {
            _gameboard = newBoard;
            boardChanged = true;
        }


        /// <summary>
        /// Display the contents of the player's hand on the console.
        /// </summary>
        public void DisplayHand()
        {
            lock (_hand)
            {
                Console.WriteLine(Environment.NewLine + "Hand:");
                for (int i = 0; i < _hand.Count; ++i)
                    Console.WriteLine("{0}: {1}", (i + 1).ToString(), _hand[i].Name);

                Console.WriteLine(Environment.NewLine);

            }
        }


        /// <summary>
        /// Displays the state of the gameboard on the console.
        /// </summary>
        public void DisplayGameboard()
        {
            Console.WriteLine("=====================================================================");

            lock (_gameboard)
            {
                for (int row = 0; row < _gameboard.GetUpperBound(0) + 1; ++row)
                {
                    for (int col = 0; col < _gameboard.GetUpperBound(1) + 1; ++col)
                    {
                        if (_gameboard[row, col] == null)
                            Console.Write("-".PadRight(12));
                        else
                            Console.Write(_gameboard[row, col].Name.PadRight(12));
                    }
                    if (row == BackRow)
                        Console.Write(" <- Your back row");

                    Console.WriteLine();
                }
            }

            Console.WriteLine("=====================================================================");
        }


        public void GetCardToPlay()
        {
            Console.WriteLine("{0}{0}Select a card to play.", Environment.NewLine);
            DisplayHand();
            int option = -1;
            bool validChoice = false;

            while(!validChoice)
            {
                option = Convert.ToInt32(Console.ReadKey().KeyChar) - 48;

                if ((option - 1) < _hand.Count && (option - 1) > -1)
                {
                    validChoice = true;
                    RequestPlayCard rsp;

                    if (_hand[option - 1].Type == "Troop")
                    {
                        Tuple<int, int> coords = GetUserCoordinates("Select where the troop should be deployed 'Row Col'");
                        rsp = new RequestPlayCard(ID, (option - 1), (coords.Item1), (coords.Item2));
                    }
                    else                    
                        rsp = new RequestPlayCard(ID, (option - 1));                        
                    
                    SendRequestToServer(rsp);
                }
                else
                { 
                    Console.WriteLine("Invalid Option\n");
                }
            }

        }


        public void GetTroopToMove()
        {
            Tuple<int, int> coords;
            Tuple<int, int> targetSpot;

            bool valid = false;
            while(!valid)
            {
                coords = GetUserCoordinates("Select one of your troops to move in the form of 'Row Col'");
                targetSpot = GetUserCoordinates("Select which cell to move to using 'Row Col'");

                if (coords.Item1 == targetSpot.Item1 && coords.Item2 == targetSpot.Item2)
                    Console.WriteLine("Current location and destination are the same.");
                else
                {
                    valid = true;
                    RequestMoveTroop rsp = new RequestMoveTroop(ID, coords.Item1, coords.Item2, targetSpot.Item1, targetSpot.Item2);
                    SendRequestToServer(rsp);
                }
            }
        }


        public void GetCellInfo()
        {
            Tuple<int, int> coords = GetUserCoordinates("Select which cell to poll using 'row col'");
            printCell(coords.Item1, coords.Item2);          

            GetPlayerAction();
        }


        public void GetTroopCombat()
        {
            Tuple<int, int> attackerCoords = GetUserCoordinates("Which troop will you attack with? 'Row Col'");
            Tuple<int, int> defenderCoords = GetUserCoordinates("Which troop will you target? 'Row Col'");
            RequestTroopCombat req = new RequestTroopCombat(ID, attackerCoords.Item1, attackerCoords.Item2, 
                defenderCoords.Item1, defenderCoords.Item2);
            SendRequestToServer(req);
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
            Disconnect();

            Console.WriteLine("Game is done. Press any key to exit.");
            Console.ReadKey();
        }


        public void BeginNextTurn(Guid turnPlayerId)
        {
            _isTurnPlayer = (ID == turnPlayerId);
            if (_isTurnPlayer)
            {
                Console.WriteLine("It is your turn.");
                GetPlayerAction();
            }
            else
            {
                Console.WriteLine("It is your opponent's turn.");
                DisplayHand();
            }
        }


        public void GetPlayerAction()
        {
            DisplayHand();
            return;
            /*
            bool validChoice = false;
            int option = -1;

            while (!validChoice)
            {
                Console.WriteLine("Please choose an Action to perform:");
                Console.WriteLine("{0} \n{1} \n{2} \n{3} \n{4}", "1. Play Card", "2. Move Troop", 
                    "3. Troop Combat", "4. Check Cell", "5. Pass Turn");
                option = Convert.ToInt32(Console.ReadKey().KeyChar) - 48;

                switch(option)
                {
                    case 1:
                        GetCardToPlay();
                        validChoice = true;
                        break;
                    case 2:
                        GetTroopToMove();
                        validChoice = true;
                        break;
                    case 3:
                        GetTroopCombat();
                        validChoice = true;
                        break;
                    case 4:
                        GetCellInfo();
                        validChoice = true;
                        break;
                    case 5:
                        PassTurn();
                        validChoice = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }*/
        }


        public void PassTurn()
        {
            RequestPassTurn req = new RequestPassTurn(ID);
            SendRequestToServer(req);
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
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using(NetworkStream netStream = _client.GetStream())
            {
                while(_shouldReceive)
                {
                    try
                    {
                        IResponse rsp = (IResponse)binaryFormatter.Deserialize(netStream);
                        ProcessResponse(rsp);
                    }
                    catch (SocketException ex)
                    {
                        WriteError("Receive socket Error: " + ex.Message);
                        Disconnect();
                    }
                    catch (IOException ex)
                    {
                        if (!isConnected())
                        {
                            WriteError("Receive IO Error: " + ex.Message);
                            Disconnect();
                        }
                    }
                    catch (System.Runtime.Serialization.SerializationException ex)
                    {
                        WriteError("Serialization Error: " + ex.Message);
                        if (!isConnected())
                            Disconnect();
                    }       
                }
            }            

            Console.WriteLine("No longer receiving messages.");
        }


        private void ProcessResponse(object obj)
        {
            IResponse serverResponse = (IResponse)obj;
            if (serverResponse is ResponseClientID) // The only response that needs special handling
                                                    // is if we're getting assigned an ID
            {
                ResponseClientID rsp = serverResponse as ResponseClientID;
                _clientId = rsp.ID;

                Console.WriteLine("ID assigned: {0}", rsp.ID.ToString());
                Console.Title = "Client " + ID.ToString();
            }
         
            else
                serverResponse.Execute(this);
        }


        private Tuple<int, int> GetUserCoordinates(string msg)
        {
            int row = 0;
            int col = 0;
            int maxRow = _gameboard.GetUpperBound(0) + 1;
            int maxCol = _gameboard.GetUpperBound(1) + 1;

            bool validChoice = false;
            string input;

            while (!validChoice)
            {
                Console.WriteLine("{0}{0}" + msg, Environment.NewLine);
                input = Console.ReadLine();
                if (input.Length == 3)
                {
                    row = (Convert.ToInt32(input[0] - 48)) - 1; // Adjust for ascii, then make zero based
                    col = (Convert.ToInt32(input[2] - 48)) - 1;

                    if (row < 0 || col < 0 || row > maxRow || col > maxCol)
                        Console.WriteLine("Please enter coordinates with the game board's bounds.");
                    else
                        validChoice = true;
                }
                else
                    Console.WriteLine("Invalid selection.");
            }

            return new Tuple<int, int>(row, col);
        }


        private void printCell(int row, int col)
        {
            if (_gameboard[row, col] == null)
                Console.WriteLine("Cell is empty");
            else
            {
                try
                {
                    _gameboard[row, col].print(ID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: {0}", ex.Message);
                }
            }                        
        }


        /// <summary>
        /// Writes the input error to both the console and a log file.
        /// </summary>
        /// <param name="error">Error to write.</param>
        public void WriteError(string error)
        {
            WriteErrorToLog(error);
            WriteErrorToConsole(error);
        }
        

        private void WriteErrorToLog(string error)
        {
            lock (_lockObject)
            {
                string path = DateTime.Now.ToString("yyyy-MM-dd") + " Client " + ID.ToString("N") + ".txt";
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
