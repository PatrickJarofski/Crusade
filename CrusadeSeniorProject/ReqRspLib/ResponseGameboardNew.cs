using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGameboardNew : IResponse
    {
        IGamePiece[,] _board;

        public ResponseGameboardNew(IGamePiece[,] board)
        {
            _board = board;
        }

        public void Execute(ICrusadeClient client)
        {
            client.SetGameboard(_board);
        }
    }
}
