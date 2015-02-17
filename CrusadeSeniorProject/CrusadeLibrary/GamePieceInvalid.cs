using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class GamePieceInvalid : GamePiece, IGamePiece
    {
        public GamePieceInvalid(int x, int y)
            :base(x, y, GamePieceType.NoType, Guid.Empty, "Invalid")
        {

        }
    }
}
