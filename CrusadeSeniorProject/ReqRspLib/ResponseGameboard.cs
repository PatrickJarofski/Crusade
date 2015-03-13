using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGameboard : IResponse
    {
        ClientGamePiece[,] _board;

        public ResponseGameboard(ClientGamePiece[,] board)
        {
            _board = board;
        }

        public void Execute(ICrusadeClient client)
        {
            client.SetGameboard(_board);
        }
    }
}
