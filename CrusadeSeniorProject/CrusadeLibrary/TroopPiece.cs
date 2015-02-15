using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class TroopPiece : GamePiece, IGamePiece
    {
        public TroopPiece(int row, int col, Guid ownerId, string name)
            :base(row, col, GamePieceType.Troop, ownerId, name)
        {

        }
    }
}
