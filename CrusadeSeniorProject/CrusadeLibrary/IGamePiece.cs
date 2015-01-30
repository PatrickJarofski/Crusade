using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public enum GamePieceType { NoType, Troop, Equip, Field };

    public interface IGamePiece
    {
        int XCoordinate { get; set; }
        int YCoordinate { get; set; }

        GamePieceType Type { get; }

        Tuple<int, int> GetCoordinates();
    }
}
