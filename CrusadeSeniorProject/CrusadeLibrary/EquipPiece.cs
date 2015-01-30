using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class EquipPiece : GamePiece, IGamePiece
    {
        public EquipPiece(int x, int y)
            :base(x, y, GamePieceType.Equip)
        {

        }
    }
}
