using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class CrusadeGame
    {
        public static readonly Random RNG = new Random();


        #region Members

        private Player _player1;
        private Player _player2;

        private Player currentPlayer;

        private Gameboard _board;

        private System.Timers.Timer debugTimer;
        private System.Timers.Timer timerToStartDebugTimer;
        
        #endregion


        #region Public Methods
        // Eventually take in two clients as parameters??
        public CrusadeGame()
        {
            _board = new Gameboard();

            _player1 = new Player();
            _player2 = new Player();

            currentPlayer = _player1;

            DealStartingHand(_player1);
            DealStartingHand(_player2);

            CreateDebugPieces();
        }

        

        public string[,] GetBoardState()
        {
            string[,] board = new string[Gameboard.BOARD_WIDTH, Gameboard.BOARD_HEIGHT];
            for(int i = 0; i < Gameboard.BOARD_WIDTH; ++i)
            {
                for(int j = 0; j < Gameboard.BOARD_HEIGHT; ++j)
                {
                    if (_board.CellOccupied(i, j))
                        board[i, j] = "occupied";
                    
                    else
                        board[i, j] = String.Empty;
                }
            }

            return board;
        }


        public Tuple<int, int> GetBoardDimensions()
        {
            Tuple<int, int> boardSize = new Tuple<int, int>(Gameboard.BOARD_HEIGHT, Gameboard.BOARD_HEIGHT);
            return boardSize;
        }


        public void BeginNextTurn()
        {
            if (currentPlayer == _player1)
                currentPlayer = _player2;

            else
                currentPlayer = _player1;

            currentPlayer.DrawFromDeck();
        }


        public Player.PlayerNumber GetCurrentPlayer()
        {
            if (currentPlayer == _player1)
                return Player.PlayerNumber.PlayerOne;
            else
                return Player.PlayerNumber.PlayerTwo;
        }


        public List<string> GetPlayerHand(Player.PlayerNumber player)
        {
            if (player == Player.PlayerNumber.NotAPlayer)
                return null;

            List<Card> cardList;
            List<string> returnList = new List<string>();

            if (player == Player.PlayerNumber.PlayerOne)
                cardList = _player1.GetHand();

            else
                cardList = _player2.GetHand();

            for(int i = 0; i < cardList.Count; ++i)            
                returnList.Add(cardList[i].ToString());
            
            return returnList;
        }


        #endregion


        #region Private Methods
        private void DealStartingHand(Player player)
        {
            for (int i = 0; i < Hand.STARTING_HAND_SIZE; ++i)
                player.DrawFromDeck();
        }


        private void CreateDebugPieces()
        {
            GamePiece debug1 = new GamePiece(2, 0);
            GamePiece debug2 = new GamePiece(2, 4);
            _board.PlaceGamePiece(debug1);
            _board.PlaceGamePiece(debug2);

            timerToStartDebugTimer = new System.Timers.Timer(10000);
            timerToStartDebugTimer.Elapsed += StartDebugTimer;
            timerToStartDebugTimer.Start();
        }

        private void StartDebugTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            debugTimer = new System.Timers.Timer(4000);
            debugTimer.Elapsed += ExtraDebugPieces;
            debugTimer.Start();
            timerToStartDebugTimer.Stop();
        }


        private void ExtraDebugPieces(object sender, System.Timers.ElapsedEventArgs e)
        {
            _board.PlaceGamePiece(new GamePiece(RNG.Next(Gameboard.BOARD_WIDTH), RNG.Next(Gameboard.BOARD_HEIGHT)));
        }

        #endregion

    }
}
