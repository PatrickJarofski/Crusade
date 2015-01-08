using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public interface ICrusadeServer
    {
        void GivePlayerHand(IGameClient client);

        IGameClient GetMatchingClient(Guid id);

        void WriteErrorToConsole(string error);

        void WriteErrorToLog(string error);
    }
}
