using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CrusadeServer
{
    public class GameClient : ReqRspLib.IGameClient
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


        public string PlayerNumber
        {
            get { return CrusadeLibrary.Player.ConvertPlayerNumberToString(_playerNumber); }
            set { _playerNumber = CrusadeLibrary.Player.ConvertStringToPlayerNumber(value); }
        }


    }
}
