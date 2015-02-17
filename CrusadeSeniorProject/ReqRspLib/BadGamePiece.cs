using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class BadGamePiece : ClientGamePiece
    {
        private string _type;

        public BadGamePiece()
        {
            RowCoordinate = -1;
            ColCoordinate = -1;
            _type = "Empty";
        }

        public int RowCoordinate { get; set; }

        public int ColCoordinate { get; set; }

        public string Type { get { return _type; } }
    }
}
