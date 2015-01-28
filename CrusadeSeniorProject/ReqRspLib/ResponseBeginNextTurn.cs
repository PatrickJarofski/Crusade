using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("Next turn player: " + _currentPlayerId.ToString());
            client.BeginNextTurn(_currentPlayerId);
        }
    }
}
