using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class CrusadeGame
    {
        #region Members

         private Player _player1;
         private Player _player2;

         private Player currentPlayer;

         private Gameboard _board;

        #endregion


        #region Methods

        // Eventually take in two clients as parameters??
        public CrusadeGame()
        {
            _board = new Gameboard();

            _player1 = new Player();
            _player2 = new Player();

            currentPlayer = _player1;

            CreateDebugPieces();
        }

        private void CreateDebugPieces()
        {
            GamePiece debug1 = new GamePiece(2, 0);
            GamePiece debug2 = new GamePiece(2, 4);
            _board.PlaceGamePiece(debug1);
            _board.PlaceGamePiece(debug2);
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


        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

        #endregion

    }
}
