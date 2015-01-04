using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class RequestHand : IRequest
    {
        private Server _server;
        
        public void Execute(Server server)
        {
            _server = server;
        }

        public RequestHand()
    }
}
