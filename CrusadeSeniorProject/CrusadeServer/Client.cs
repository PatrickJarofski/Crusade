using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using PlayerNumber = CrusadeLibrary.Player.PlayerNumber;

namespace CrusadeServer
{
    public class Client
    {
        public Socket clientSocket;

        private PlayerNumber _playerNumber;
        public PlayerNumber PlayerID
        {
            get { return _playerNumber; }
            set { _playerNumber = value; }
        }

        public Client(Socket socket)
        {
            clientSocket = socket;
            PlayerID = PlayerNumber.NotAPlayer;           // Indicates this Client has
                                                          // not been assigned an ID                                    
        }
    }
}
