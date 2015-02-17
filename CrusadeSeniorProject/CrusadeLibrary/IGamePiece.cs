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
        string Name { get; }

        int RowCoordinate { get; set; }

        int ColCoordinate { get; set; }

        GamePieceType Type { get; }

        Tuple<int, int> GetCoordinates();
    }
}
