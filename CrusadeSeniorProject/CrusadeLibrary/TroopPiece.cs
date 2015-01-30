using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class TroopPiece : GamePiece, IGamePiece
    {
        public TroopPiece(int x, int y)
            :base(x, y, GamePieceType.Troop)
        {

        }
    }
}
