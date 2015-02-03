using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class BadGamePiece : IGamePiece
    {
        private byte _type;

        public BadGamePiece()
        {
            RowCoordinate = -1;
            ColCoordinate = -1;
            _type = Constants.BAD_ATTRIBUTE;
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
