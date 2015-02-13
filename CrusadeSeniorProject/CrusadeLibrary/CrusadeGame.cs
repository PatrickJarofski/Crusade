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

        private Gameboard _board;

        #endregion


        #region Properties

        internal Gameboard Board { get { return _board; } }

        internal Player Player1 { get { return _player1; } }
        internal Player Player2 { get { return _player2; } }

        internal Player CurrentPlayer { get; set; }

        internal State CurrentState { get; set; }

        #endregion


        #region Public Methods
        public CrusadeGame()
        {
            _board = new Gameboard();
            _player1 = new Player();
            _player2 = new Player();

            CurrentState = new StateNewGame().entry(this, null);
        }

        public CrusadeGame(Guid player1Id, Guid player2Id)
        {
            _board = new Gameboard();
            _player1 = new Player(player1Id);
            _player2 = new Player(player2Id);

            CurrentState = new StateNewGame().entry(this, null);
        }


        public object performAction(object obj)
        {
            CurrentState = CurrentState.entry(this, obj);
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


        public ICard PlayCard(Guid playerId, int cardSlot)
        {
            try
            {
                return CurrentState.PlayCard(this, playerId, cardSlot);
            }
            catch(CrusadeLibrary.GameStateException ex)
            {
                throw new FormatException(ex.Message);
            }
            catch(CrusadeLibrary.IllegalActionException ex)
            {
                throw new IllegalActionException(ex.Message);
            }
        }


        public ICard PlayCard(Guid playerId, int cardSlot, int row, int col)
        {
            try
            {
                return CurrentState.PlayCard(this, playerId, cardSlot, row, col);
            }
            catch (CrusadeLibrary.GameStateException ex)
            {
                throw new FormatException(ex.Message);
            }
        }


        #endregion
    }
}
