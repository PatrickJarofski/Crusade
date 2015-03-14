using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestPassTurn : IRequest
    {
        Guid id;

        public RequestPassTurn(Guid clientId)
        {
            id = clientId;
        }

        public void Execute(ICrusadeServer server)
        {
            server.PassTurn(id);
        }
    }
}
