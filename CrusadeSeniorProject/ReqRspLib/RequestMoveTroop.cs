using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestMoveTroop : IRequest
    {
        int _sRow;
        int _sCol;
        int _eRow;
        int _eCol;

        Guid _id;

        public RequestMoveTroop(Guid playerId, int startRow, int startCol, int endRow, int endCol)
        {
            _id = playerId;
            _sRow = startRow;
            _sCol = startCol;
            _eRow = endRow;
            _eCol = endCol;
        }

        public void Execute(ICrusadeServer server)
        {
            server.MoveTroop(_id, _sRow, _sCol, _eRow, _eCol);
        }
    }
}
