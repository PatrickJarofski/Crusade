using System;

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
