using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CrusadeServer
{
    public class GameClient
    {
        private readonly TcpClient _client;
        private CrusadeLibrary.Player.PlayerNumber _playerNumber;
        private Guid _id;

        public Guid ID { get { return _id; } }

        internal GameClient(TcpClient client, Guid id)
        {
            _id = id;
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
