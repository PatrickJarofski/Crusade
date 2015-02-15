using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGameboard : IResponse
    {
        IGamePiece[,] _board;

        public ResponseGameboard(IGamePiece[,] board)
        {
            _board = board;
        }

        public void Execute(ICrusadeClient client)
        {
            client.SetGameboard(_board);
        }
    }
}
