using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ReqRspLib;

namespace CrusadeServer
{
    public partial class Server : ReqRspLib.ICrusadeServer
    {
        /// <summary>
        /// Obtains the hand of the given GameClient, then sends
        /// a response back to the client with that hand's information.
        /// </summary>
        /// <param name="client">The GameClient to respond to.</param>
        public void GivePlayerHand(Guid clientId)
        {
            GameClient client = GetMatchingClient(clientId);

            lock (lockObject)
            {
                if (_game == null)
                {
                    Console.WriteLine("Game is null. Ignoring request.");
                    return;
                }
            }

            try
            {
                List<CrusadeLibrary.Card> hand = _game.GetPlayerHand(client.PlayerNumber);
                Converter<CrusadeLibrary.ICard, ReqRspLib.ICard> con = new Converter<CrusadeLibrary.ICard, ReqRspLib.ICard>(ConvertToRspICard);
                List<ReqRspLib.ICard> handToShip = hand.ConvertAll(con);

                ResponseHand rsp = new ResponseHand(handToShip);
                SendData(client, rsp);
            }
            catch(NullReferenceException ex)
            {
                WriteErrorToConsole("Give Player Hand error: " + ex.Message);
                WriteErrorToLog("Give Player Hand error: " + ex.Message);
            }
        }


        /// <summary>
        /// Give the client the state of the gameboard
        /// </summary>
        /// <param name="clientId">Id of the player/client.</param>
        public void GivePlayerGameboard(Guid clientId)
        {
            string[,] board = _game.GetBoardState();
            ResponseGameboard rsp = new ResponseGameboard(board);
            SendData(GetMatchingClient(clientId), rsp);
        }


        /// <summary>
        /// Play a card that is in a Client's hand.
        /// </summary>
        /// <param name="client">The Client playing the card.</param>
        /// <param name="cardNum">The index of the card in the hand.</param>
        public void PlayCard(Guid clientId, int cardNum)
        {
            CrusadeLibrary.ICard card = _game.PlayCard((GetMatchingClient(clientId).PlayerNumber), cardNum);

            ResponsePlayCard rsp = new ResponsePlayCard(ConvertToRspICard(card));
            BroadcastToClients(rsp);
            BeginNextTurn();
        }

        /// <summary>
        /// Play a card that is in a Client's hand.
        /// </summary>
        /// <param name="clientId">The Client playing the card.</param>
        /// <param name="cardNum">The index of the card in the hand.</param>
        /// <param name="x">Target X Coordinate for the card.</param>
        /// <param name="y">Target Y Coordinate for the card.</param>
        public void PlayCard(Guid clientId, int cardNum, int row, int col)
        {
            CrusadeLibrary.ICard card = _game.PlayCard((GetMatchingClient(clientId).PlayerNumber), cardNum, row, col);

            ResponsePlayCard rsp = new ResponsePlayCard(ConvertToRspICard(card), row + 1, col + 1); // +1 Since user expects one based coordinates
            BroadcastToClients(rsp);
            BeginNextTurn();
        }


        /// <summary>
        /// Writes the given string to the server's error log.
        /// </summary>
        /// <param name="error">String to write</param>
        public void WriteErrorToLog(string error)
        {
            lock (lockObject)
            {
                string path = DateTime.Now.ToString("yyyy-MM-dd" + " Server Log");
                string msg = DateTime.Now.ToString("hh:mm:ss ") + error;
                System.IO.File.AppendAllText(path, msg);
            }
        }


        /// <summary>
        /// Writes a given string to the console with additional formatting for readability.
        /// </summary>
        /// <param name="error">String to write.</param>
        public void WriteErrorToConsole(string error)
        {
            lock (lockObject)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("====================================================================");
                Console.WriteLine(error);
                Console.WriteLine("====================================================================");
                Console.WriteLine(Environment.NewLine);
            }
        }


        /// <summary>
        /// Gets the GameClient that matches the given Guid.
        /// </summary>
        /// <param name="ep">Guid to match</param>
        /// <returns>GameClient whose Guid matches the given Guid.</returns>
        private GameClient GetMatchingClient(Guid id)
        {
            foreach (GameClient client in _clientList)
            {
                if (client.ID == id)
                    return client;
            }

            throw new NullReferenceException("The given ID does not match any GameClients.");
        }


        /// <summary>
        /// Begins the next turn of the game, notifying clients of the change.
        /// </summary>
        private void BeginNextTurn()
        {
            lock (_clientList)
            {
                foreach (GameClient client in _clientList)
                {
                    if (client.isTurnPlayer == true)
                        client.isTurnPlayer = false;
                    else
                        client.isTurnPlayer = true;
                }
            }

            _game.BeginNextTurn();

            foreach (GameClient client in _clientList.ToArray())
            {
                GivePlayerHand(client.ID);
                GivePlayerGameboard(client.ID);
            }

            ResponseBeginNextTurn rsp = new ResponseBeginNextTurn(GetTurnPlayerId());
            BroadcastToClients(rsp);
        }


        /// <summary>
        /// Finds the Client that is currently the turn player.
        /// </summary>
        /// <returns>The ID of the turn client.</returns>
        private Guid GetTurnPlayerId()
        {
            foreach (GameClient client in _clientList.ToArray())
            {
                if (client.isTurnPlayer)
                    return client.ID;
            }
            throw new ArgumentException("The player number specified does not exist.");
        }



        private ReqRspLib.ICard ConvertToRspICard(CrusadeLibrary.ICard card)
        {
            ReqRspLib.CardType newType;
            if (card.Type == CrusadeLibrary.CardType.Troop)
                newType = ReqRspLib.CardType.Troop;
            else if
                (card.Type == CrusadeLibrary.CardType.Equip)
                newType = ReqRspLib.CardType.Equip;
            else
                newType = ReqRspLib.CardType.Field;

            return new ReqRspLib.Card(card.Name, newType);
        }

    }
}
