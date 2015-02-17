using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class TroopGamePiece : ClientGamePiece
    {
        private string _type;

        public TroopGamePiece(string type, int row, int col)
        {
            _type = type;
            RowCoordinate = row;
            ColCoordinate = col;
        }

        public int RowCoordinate { get; set; }

        public int ColCoordinate { get; set; }

        public string Type { get { return _type; } }
    }
}
