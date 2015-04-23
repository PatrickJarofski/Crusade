using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseBeginNextTurn : IResponse
    {
        Guid _currentPlayerId;
        int AP;

        public ResponseBeginNextTurn(Guid currentPlayerId, int ap)
        {
            _currentPlayerId = currentPlayerId;
            AP = ap;
        }

        public void Execute(ICrusadeClient client)
        {
            client.ActionPoints = AP;
            client.BeginNextTurn(_currentPlayerId);
        }
    }
}
