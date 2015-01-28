using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGameboard : IResponse
    {
        string[,] _board;

        public ResponseGameboard(string[,] board)
        {
            _board = board;
        }

        public void Execute(ICrusadeClient client)
        {
            client.SetGameboard(_board);
        }
    }
}
