using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Gameboard
    {
        public const int BOARD_WIDTH = 6;
        public const int BOARD_HEIGHT = 6;

        private GamePiece[,] _board;


        /// <summary>
        /// Constructor
        /// </summary>
        public Gameboard()
        {
            _board = new GamePiece[BOARD_WIDTH, BOARD_HEIGHT];
        }


        /// <summary>
        /// Place a game piece on the board
        /// </summary>
        /// <param name="piece">GamePiece to place</param>
        public void PlaceGamePiece(GamePiece piece)
        {
            Tuple<int, int> coordinates = piece.GetCoordinates();
            _board[coordinates.Item1, coordinates.Item2] = piece;
        }


        /// <summary>
        /// Remove a game piece from specified coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void RemoveGamePiece(int x, int y)
        {
            _board[x, y] = null;
        }



    }
}
