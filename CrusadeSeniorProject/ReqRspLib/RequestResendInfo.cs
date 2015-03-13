using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestResendInfo : IRequest
    {
        Guid _id;

        public RequestResendInfo(Guid clientId)
        {
            _id = clientId;
        }

        public void Execute(ICrusadeServer server)
        {
            server.GivePlayerGameboard(_id);
            server.GivePlayerHand(_id);
        }
    }
}
