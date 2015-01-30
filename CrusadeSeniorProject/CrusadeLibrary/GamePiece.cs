using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public abstract class GamePiece : BaseGameObject, IGamePiece
    {
        private int _xCoordinate;
        private int _yCoordinate;
        private GamePieceType _type;

        public int XCoordinate
        {
            get { return _xCoordinate; }
            set { _xCoordinate = value; }
        }

        public int YCoordinate
        {
            get { return _yCoordinate; }
            set { _yCoordinate = value; }
        }

        public GamePieceType Type 
        { 
            get { return _type; } 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Starting x coordinate</param>
        /// <param name="y">Starting y coordinate</param>
        public GamePiece(int x, int y, GamePieceType type)
        {
            _xCoordinate = x;
            _yCoordinate = y;
            _type = type;
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
