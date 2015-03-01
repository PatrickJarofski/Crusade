using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseStartGame : IResponse
    {
        int _backRow;

        public ResponseStartGame(int clientBackRow)
        {
            _backRow = clientBackRow;
        }

        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine("Start game.\n");
            client.BeginGame();
            client.BackRow = _backRow;
        }
    }
}
