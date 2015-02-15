using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class FieldPiece : GamePiece, IGamePiece
    {
        public FieldPiece(int x, int y, Guid ownerId, string name)
            :base(x, y, GamePieceType.Field, ownerId, name)
        {

        }
    }
}
