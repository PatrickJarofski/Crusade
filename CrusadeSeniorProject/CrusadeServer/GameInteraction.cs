using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public partial class Server
    {
        public void GivePlayerHand(TcpClient tcpClient)
        {
            if(_game != null)
            {
                GameClient client = GetMatchingClient(tcpClient);
                List<string> hand = _game.GetPlayerHand(client.PlayerNumber);

            }            
        }
    }
}
