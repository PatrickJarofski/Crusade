using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CrusadeServer
{
    internal class GameClient
    {
        private readonly TcpClient _client;
        private CrusadeLibrary.Player.PlayerNumber _playerNumber;

        internal GameClient(TcpClient client)
        {
            _client = client;
            _playerNumber = CrusadeLibrary.Player.PlayerNumber.NotAPlayer;
        }


        public TcpClient TCPclient
        {
            get { return _client; }
        }


        public CrusadeLibrary.Player.PlayerNumber PlayerNumber
        {
            get { return _playerNumber; }
            set { _playerNumber = value; }
        }

    }
}
