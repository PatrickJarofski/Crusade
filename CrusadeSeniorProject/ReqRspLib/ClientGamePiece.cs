using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public class ClientGamePiece
    {
        public string Name { get; set; }

        public int RowCoordinate { get; set; }

        public int ColCoordinate { get; set; }

        public string Type { get; set; }

        public ClientGamePiece()
        {
            Name = string.Empty;
            Type = string.Empty;
        }

    }
}
