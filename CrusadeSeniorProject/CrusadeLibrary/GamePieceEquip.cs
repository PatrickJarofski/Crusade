using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class GamePieceEquip : GamePiece, IGamePiece
    {
        public GamePieceEquip(int x, int y, Guid ownerId, string name)
            :base(x, y, GamePieceType.Equip, ownerId, name)
        {

        }
    }
}
