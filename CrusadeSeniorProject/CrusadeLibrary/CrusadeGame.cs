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

        internal Player Player1 { get; set; }
        internal Player Player2 { get; set; }

        internal Player CurrentPlayer { get; set; }

        internal Gameboard _board;

        private State _currentState;

        #endregion


        #region Public Methods
        public CrusadeGame()
        {
            _board = new Gameboard();
            Player1 = new Player();
            Player2 = new Player();

            _currentState = new StateNewGame().performAction(this, null);
        }


        public object performAction(object obj)
        {
            _currentState = _currentState.performAction(this, obj);
            return null; // Dunno
        }


        public IGamePiece[,] GetBoardState()
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
            if (CurrentPlayer == Player1)
                CurrentPlayer = Player2;

            else // currentPlayer == _player2
                CurrentPlayer = Player1;

            CurrentPlayer.DrawFromDeck();
        }


        public Player.PlayerNumber GetCurrentPlayer()
        {
            if (CurrentPlayer == Player1)
                return Player.PlayerNumber.PlayerOne;
            else
                return Player.PlayerNumber.PlayerTwo;
        }


        public List<Card> GetPlayerHand(Player.PlayerNumber player)
        {
            if (player == Player.PlayerNumber.NotAPlayer)
                return null;

            if (player == Player.PlayerNumber.PlayerOne)
                return Player1.GetHand();

            else
                return Player2.GetHand();
        }


        public ICard PlayCard(Player.PlayerNumber player, int cardSlot)
        {
            if (player == Player.PlayerNumber.PlayerOne)
               return Player1.PlayCard(cardSlot);

            else if (player == Player.PlayerNumber.PlayerTwo)
              return Player2.PlayCard(cardSlot);

            else
                throw new FormatException("An invalid player number was encountered.");
        }


        public ICard PlayCard(Player.PlayerNumber player, int cardSlot, int row, int col)
        {
            if (player == Player.PlayerNumber.PlayerOne)
            {
                _board.PlaceGamePiece(new TroopPiece(row, col));
                return Player1.PlayCard(cardSlot);
            }

            else if (player == Player.PlayerNumber.PlayerTwo)
            {
                _board.PlaceGamePiece(new TroopPiece(row, col));
                return Player2.PlayCard(cardSlot);
            }

            else
                throw new FormatException("An invalid player number was encountered.");
        }


        #endregion
    }
}
