using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Gameboard : BaseGameObject
    {
        public const int BOARD_COL = 5;
        public const int BOARD_ROW = 5;

        private IGamePiece[,] _board;


        /// <summary>
        /// Constructor
        /// </summary>
        public Gameboard()
        {
            _board = new IGamePiece[BOARD_ROW, BOARD_COL];
        }


        /// <summary>
        /// Place a game piece on the board
        /// </summary>
        /// <param name="piece">GamePiece to place</param>
        public void PlaceGamePiece(IGamePiece piece)
        {
            Tuple<int, int> coordinates = piece.GetCoordinates();
            if(!CellOccupied(coordinates.Item1, coordinates.Item2))
                _board[coordinates.Item1, coordinates.Item2] = piece;
        }


        /// <summary>
        /// Remove a game piece from specified coordinates
        /// </summary>
        /// <param name="row">Row coordinate</param>
        /// <param name="col">Column coordinate</param>
        public void RemoveGamePiece(int row, int col)
        {
            _board[row, col] = null;
        }


        public bool CellOccupied(int row, int col)
        {
            if (_board[row, col] != null)
                return true;

            else
                return false;
        }


        public IGamePiece GetPiece(int row, int col)
        {
            try
            {
                return _board[row, col];
            }
            catch
            {
                return new InvalidPiece(0, 0);
            }
        }
    }
}
