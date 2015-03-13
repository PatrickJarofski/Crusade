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
            client.BeginNextTurn(_currentPlayerId);
        }
    }
}
