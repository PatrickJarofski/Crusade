using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class GamePiece : BaseGameObject
    {
        private int _xCoordinate;
        private int _yCoordinate;

        // Type of Piece here

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Starting x coordinate</param>
        /// <param name="y">Starting y coordinate</param>
        public GamePiece(int x, int y)
        {
            _xCoordinate = x;
            _yCoordinate = y;
        }

        /// <summary>
        /// Gets the coordinates of the game piece
        /// </summary>
        /// <returns>A tuple of 2 ints where the first
        /// int is the x coordinate and the second is the y.</returns>
        public Tuple<int, int> GetCoordinates()
        {
            return new Tuple<int, int>(_xCoordinate, _yCoordinate);
        }
    }
}
