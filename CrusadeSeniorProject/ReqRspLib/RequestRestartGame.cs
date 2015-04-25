using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestRestartGame : IRequest
    {
        Guid player;

        public RequestRestartGame(Guid id)
        {
            player = id;
        }

        public void Execute(ICrusadeServer server)
        {
            server.RestartGame(player);
        }
    }
}
