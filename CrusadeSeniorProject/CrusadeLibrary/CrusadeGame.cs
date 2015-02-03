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
            string[,] board = new string[Gameboard.BOARD_ROW, Gameboard.BOARD_COL];
            for (int row = 0; row < Gameboard.BOARD_ROW; ++row)
            {
                for (int col = 0; col < Gameboard.BOARD_COL; ++col)
                {
                    if (_board.CellOccupied(row, col))
                        board[row, col] = "O";

                    else
                        board[row, col] = "E";
                }
                if (board[row, Gameboard.BOARD_COL - 1] == "E")
                    board[row, Gameboard.BOARD_COL - 1] = "E\n";
                else
                    board[row, Gameboard.BOARD_COL - 1] = "O\n";
                
            }

            return board;
        }

        public IGamePiece[,] GetBoardStateNew()
        {
            IGamePiece[,] board = new IGamePiece[Gameboard.BOARD_ROW, Gameboard.BOARD_COL];

            for (int row = 0; row < Gameboard.BOARD_ROW; ++row)
            {
                for(int col = 0; col < Gameboard.BOARD_COL; ++col)
                {
                    if (_board.CellOccupied(row, col))
                        board[row, col] = _board.GetPiece(row, col);

                    else
                        board[row, col] = null;
                }
            }

            return board;
        }


        public Tuple<int, int> GetBoardDimensions()
        {
            Tuple<int, int> boardSize = new Tuple<int, int>(Gameboard.BOARD_ROW, Gameboard.BOARD_COL);
            return boardSize;
        }


        public void BeginNextTurn()
        {
            if (currentPlayer == _player1)
                currentPlayer = _player2;

            else // currentPlayer == _player2
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


        public List<Card> GetPlayerHand(Player.PlayerNumber player)
        {
            if (player == Player.PlayerNumber.NotAPlayer)
                return null;

            if (player == Player.PlayerNumber.PlayerOne)
                return _player1.GetHand();

            else
                return _player2.GetHand();
        }


        public ICard PlayCard(Player.PlayerNumber player, int cardSlot)
        {
            if (player == Player.PlayerNumber.PlayerOne)
               return _player1.PlayCard(cardSlot);

            else if (player == Player.PlayerNumber.PlayerTwo)
              return _player2.PlayCard(cardSlot);

            else
                throw new FormatException("An invalid player number was encountered.");
        }


        public ICard PlayCard(Player.PlayerNumber player, int cardSlot, int row, int col)
        {
            if (player == Player.PlayerNumber.PlayerOne)
            {
                _board.PlaceGamePiece(new TroopPiece(row, col));
                return _player1.PlayCard(cardSlot);
            }

            else if (player == Player.PlayerNumber.PlayerTwo)
            {
                _board.PlaceGamePiece(new TroopPiece(row, col));
                return _player2.PlayCard(cardSlot);
            }

            else
                throw new FormatException("An invalid player number was encountered.");
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
            TroopPiece debug1 = new TroopPiece(0, 2);
            TroopPiece debug2 = new TroopPiece(4, 2);
            _board.PlaceGamePiece(debug1);
            _board.PlaceGamePiece(debug2);
        }

        #endregion
    }
}
