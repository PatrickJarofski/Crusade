using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

                List<string> stringHand = GetJsonCardList(hand);

         //       Converter<CrusadeLibrary.ICard, ReqRspLib.ICard> con = new Converter<CrusadeLibrary.ICard, ReqRspLib.ICard>(ConvertToRspICard);
         //       List<ReqRspLib.ICard> handToShip = hand.ConvertAll(con);

                ResponseHand rsp = new ResponseHand(stringHand);
                SendData(client, rsp);
            }
            catch (NullReferenceException ex)
            {
                WriteErrorToConsole("Give Player Hand error: " + ex.Message);
                WriteErrorToLog("Give Player Hand error: " + ex.Message);
            }
        }


        private List<string> GetJsonCardList(List<CrusadeLibrary.Card> list)
        {
            List<string> newList = new List<string>();

            foreach(CrusadeLibrary.Card card in list)
            {
                newList.Add(JsonConvert.SerializeObject(card));
            }

            return newList;
        }


        public void GivePlayerGameboard(Guid clientId)
        {
            CrusadeLibrary.IGamePiece[,] board = _game.GetBoardState();

            int numRows = board.GetUpperBound(0) + 1; // GetUpperBound() returns the highest # index
            int numCols = board.GetUpperBound(1) + 1; // for the dimension specified. +1 to it make one-based

            string[,] convertedBoard = new string[numRows, numCols];
            for(int row = 0; row < numRows; ++row)
            {
                for(int col = 0; col < numCols; ++col)
                {
                    convertedBoard[row, col] = JsonConvert.SerializeObject(board[row, col]);
                }
            }

            ResponseGameboard rsp = new ResponseGameboard(convertedBoard);
            SendData(clientId, rsp);
        }


        public void GiveAllPlayersHand()
        {
            foreach (GameClient client in _clientList.ToArray())
                GivePlayerHand(client.ID);
        }

        public void GiveAllPlayersGameboard()
        {
            foreach (GameClient client in _clientList.ToArray())
            {
                GivePlayerGameboard(client.ID);
                ResponseDisplayGameboard rsp = new ResponseDisplayGameboard();
                SendData(client, rsp);
            }
        }


        /// <summary>
        /// Play a card that is in a Client's hand.
        /// </summary>
        /// <param name="client">The Client playing the card.</param>
        /// <param name="cardNum">The index of the card in the hand.</param>
        public void PlayCard(Guid clientId, int cardNum)
        {
            try
            {
                Tuple<CrusadeLibrary.ICard, bool> value = _game.PlayCard(clientId, cardNum);

                ResponsePlayCard rsp = new ResponsePlayCard(value.Item1.Name);
                BroadcastToClients(rsp);

                if(value.Item2)
                    BeginNextTurn();
                else
                    GetNextPlayerAction(clientId);                
            }
            catch(NotImplementedException ex)
            {
                SendBadCardChoiceError(ex.Message, clientId);
            }
            catch(CrusadeLibrary.IllegalActionException ex)
            {
                SendBadCardChoiceError(ex.Message, clientId);
            }
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
            try
            {
                Tuple<CrusadeLibrary.ICard, bool> value = _game.PlayCard(clientId, cardNum, row, col);

                ResponsePlayCard rsp = new ResponsePlayCard(value.Item1.Name, row, col);
                BroadcastToClients(rsp);

                if (value.Item2)
                    BeginNextTurn();
                else
                    GetNextPlayerAction(clientId);
            }
            catch(CrusadeLibrary.IllegalActionException ex)
            {
                SendBadCardChoiceError(ex.Message, clientId);
            }
            catch(NotImplementedException ex)
            {
                SendBadCardChoiceError(ex.Message, clientId);
            }
        }


        private void GetNextPlayerAction(Guid clientId)
        {
            GivePlayerHand(clientId);

            GiveAllPlayersGameboard();

            ResponseMessage msg = new ResponseMessage("\nRemaining Action Points: " + _game.CurrentPlayerAP.ToString());
            SendData(clientId, msg);

            ResponseGetCardToPlay rspGetMove = new ResponseGetCardToPlay();
            SendData(clientId, rspGetMove);
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

            GiveAllPlayersGameboard();
            GiveAllPlayersHand();

            ResponseBeginNextTurn rsp = new ResponseBeginNextTurn(_game.CurrentPlayerId);
            BroadcastToClients(rsp);
        }       

        /*
        /// <summary>
        /// Convert a CrusadeLibrary.ICard to a ReqRspLib.ICard
        /// </summary>
        /// <param name="card">ICard to convert.</param>
        /// <returns></returns>
        private ReqRspLib.ICard ConvertToRspICard(CrusadeLibrary.ICard card)
        {
            byte newType;
            if (card.Type == CrusadeLibrary.CardType.Troop)
                newType = ReqRspLib.Constants.CARD_TYPE_TROOP;
            else if (card.Type == CrusadeLibrary.CardType.Equip)
                newType = ReqRspLib.Constants.CARD_TYPE_EQUIP;
            else
                newType = ReqRspLib.Constants.CARD_TYPE_FIELD;

            return new ReqRspLib.Card(card.Name, newType);
        }
        */

        /*
        /// <summary>
        /// Convert a CrusadeLibrary.IGamePiece to a ReqRspLib.IGamePiece
        /// </summary>
        /// <param name="piece">IGamePiece to convert.</param>
        /// <returns></returns>
        private ReqRspLib.IGamePiece ConvertToRspIGamePiece(CrusadeLibrary.IGamePiece piece)
        {
            if (piece == null)
                return new ReqRspLib.BadGamePiece();

            CrusadeLibrary.GamePieceType type = piece.Type;

            switch (type)
            {
                case CrusadeLibrary.GamePieceType.Troop:
                    return new ReqRspLib.TroopGamePiece("Troop", piece.RowCoordinate, piece.ColCoordinate);
                default:
                    return new ReqRspLib.BadGamePiece();
            }

        }
        */

        private void SendBadCardChoiceError(string er, Guid clientId)
        {
            ResponseBadMove rsp = new ResponseBadMove(er);
            SendData(clientId, rsp);

            ResponseGetCardToPlay rsp2 = new ResponseGetCardToPlay(); // Resend the request since the last one failed.
            SendData(clientId, rsp2);
        }
    }
}
