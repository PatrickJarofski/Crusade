using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public abstract class GamePiece : BaseGameObject, IGamePiece
    {
        private int _rowCoordinate;
        private int _colCoordinate;
        private GamePieceType _type;
        private Guid _ownerId;
        private string _name;

        public Guid OwnerID { get { return _ownerId; } }

        public string Name { get { return _name; } }


        public int RowCoordinate
        {
            get { return _rowCoordinate; }
            set { _rowCoordinate = value; }
        }

        public int ColCoordinate
        {
            get { return _colCoordinate; }
            set { _colCoordinate = value; }
        }

        public GamePieceType Type 
        { 
            get { return _type; } 
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="row">Starting row coordinate</param>
        /// <param name="col">Starting column coordinate</param>
        public GamePiece(int row, int col, GamePieceType type, Guid ownerId, string name)
        {
            _name = name;
            _ownerId = ownerId;
            _rowCoordinate = row;
            _colCoordinate = col;
            _type = type;
        }

        /// <summary>
        /// Gets the coordinates of the game piece
        /// </summary>
        /// <returns>A tuple of 2 ints where the first
        /// int is the x coordinate and the second is the y.</returns>
        public Tuple<int, int> GetCoordinates()
        {
            return new Tuple<int, int>(_rowCoordinate, _colCoordinate);
        }
    }
}
