using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestGameboard : IRequest
    {
        private Guid _id;

        public RequestGameboard(Guid id)
        {
            _id = id;
        }

        public void Execute(ICrusadeServer server)
        {
            server.GivePlayerGameboard(_id);
        }
    }
}
