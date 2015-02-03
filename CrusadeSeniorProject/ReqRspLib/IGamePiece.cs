using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public interface IGamePiece
    {
        int RowCoordinate { get; set; }

        int ColCoordinate { get; set; }

        string Type { get; }

    }
}
