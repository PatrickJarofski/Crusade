using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class TroopGamePiece : IGamePiece
    {
        private byte _type;

        public TroopGamePiece(byte type, int row, int col)
        {
            _type = type;
            RowCoordinate = row;
            ColCoordinate = col;
        }

        public int RowCoordinate { get; set; }

        public int ColCoordinate { get; set; }

        public byte Type { get { return _type; } }

        public Tuple<int, int> GetCoordinates()
        {
            return new Tuple<int, int>(RowCoordinate, ColCoordinate);
        }
    }
}
