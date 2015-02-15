using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseBeginNextTurn : IResponse
    {
        Guid _currentPlayerId;

        public ResponseBeginNextTurn(Guid currentPlayerId)
        {
            _currentPlayerId = currentPlayerId;
        }

        public void Execute(ICrusadeClient client)
        {
            if(client.ID == _currentPlayerId)
            {
                client.IsTurnPlayer = true;
                Console.WriteLine("It is your turn.");
            }
            else
            {
                client.IsTurnPlayer = false;
                Console.WriteLine("It is your opponent's turn.");
            }
                
            client.BeginNextTurn();
        }
    }
}
