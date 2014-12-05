using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using CrusadeLibrary;
using System.Threading;

using PlayerNumber = CrusadeLibrary.Player.PlayerNumber;

namespace CrusadeServer
{
    partial class Server
    {
        private void BeginNewGame()
        {
            int playerOne = CrusadeLibrary.CrusadeGame.RNG.Next(0, 2);

            if (playerOne == 1)
            {
                _clientList[0].PlayerID = PlayerNumber.PlayerOne;
                _clientList[1].PlayerID = PlayerNumber.PlayerTwo;
            }
            else
            {
                _clientList[1].PlayerID = PlayerNumber.PlayerOne;
                _clientList[0].PlayerID = PlayerNumber.PlayerTwo;
            }

            _Game = new CrusadeGame();

            Console.WriteLine("Game has started.");
            UpdateAllClients(GenerateResponse(ResponseTypes.ClientResponse, Responses.GameStarted));
        }


        private void SendPlayerHand(Player.PlayerNumber player)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string card in _Game.GetPlayerHand(player))
                sb.Append(card + '\n');

            SendData(GetMatchingClient(player), GenerateResponse(ResponseTypes.GameResponse, Responses.GiveHand + Constants.GameResponseDelimiter + sb.ToString()));
        }


        private void ShutdownGame()
        {
            _Game = null;
            lock(_clientList)
            {
                foreach (Client client in _clientList)
                    client.PlayerID = PlayerNumber.NotAPlayer;
            }

            Console.WriteLine("Game has ended.");
            foreach (Client client in _clientList)
            {
                SendData(client, GenerateResponse(ResponseTypes.ClientResponse, Responses.GameOver));
            }
        }


        private void UpdateAllClients(JSONResponse jsonResponse)
        {
            foreach (Client client in _clientList.ToArray())
                SendData(client, jsonResponse);
        }


        internal void GiveClientBoardState(Client client)
        {
            if (_Game == null)
                return;

            string[,] board = _Game.GetBoardState();
            Tuple<int, int> boardSize = _Game.GetBoardDimensions();

            StringBuilder sb = new StringBuilder();

            sb.Append(boardSize.Item1.ToString());
            sb.Append('|');
            sb.Append(boardSize.Item2.ToString());
            sb.Append('|');

            for(int i = 0; i < boardSize.Item1; ++i)
            {
                for(int j = 0; j < boardSize.Item2; ++j)
                {
                    if (board[i, j] != String.Empty)
                        sb.Append('O');

                    else
                        sb.Append('E');

                    sb.Append('|');
                }
                sb.Append("=|");
            }

            SendData(client, GenerateResponse(ResponseTypes.GameResponse, Responses.GiveGameboard + 
                Constants.GameResponseDelimiter + sb.ToString() ));
        }

        private void PlayCard(Client client, string cardSlot)
        {
            int card = Convert.ToInt32(cardSlot);
            string cardPlayed = _Game.PlayCard(client.PlayerID, card);
            UpdateAllClients(GenerateResponse(ResponseTypes.GameResponse, Responses.CardPlayed + Constants.GameResponseDelimiter + cardPlayed));
        }
    }
}
