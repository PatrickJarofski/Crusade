using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CrusadeServer
{
    public class RequestHand : IRequest
    {
        private TcpClient _client;

        public TcpClient Client
        {
            get { return _client; }
        }
        
        public void Execute(Server server)
        {
            // server.GivePlayerHand(_client);
        }

        public RequestHand(TcpClient client)
        {
            _client = client;
        }
    }
}
