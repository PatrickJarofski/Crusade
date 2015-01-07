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
        /// <summary>
        /// Obtains the hand of the given GameClient, then sends
        /// a response back to the client with that hand's information.
        /// </summary>
        /// <param name="client">The GameClient to respond to.</param>
        public void GivePlayerHand(GameClient client)
        {
            if (_game == null)
                return;

            try
            {
                List<string> hand = _game.GetPlayerHand(client.PlayerNumber);

                ResponseHand rsp = new ResponseHand(hand);
                SendData(client, rsp);
            }
            catch(NullReferenceException ex)
            {
                WriteErrorToConsole("Give Player Hand error: " + ex.Message);
                WriteErrorToLog("Give Player Hand error: " + ex.Message);
            }
        }
    }
}
