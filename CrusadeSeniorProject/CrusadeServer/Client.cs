using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class Client
    {
        public enum PlayerNumber { PlayerOne, PlayerTwo, NotAPlayer };

        public Socket clientSocket;

        private PlayerNumber _playerNumber;
        public PlayerNumber PlayerID
        {
            get { return _playerNumber; }
            set { _playerNumber = value; }
        }

        public Client(ref Socket socket)
        {
            clientSocket = socket;
            PlayerID = PlayerNumber.NotAPlayer;           // Indicates this Client has
                                                          // not been assigned an ID                                    
        }
    }
}
