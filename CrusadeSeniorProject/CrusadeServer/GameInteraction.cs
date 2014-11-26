using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using CrusadeLibrary;
using System.Threading;

namespace CrusadeServer
{
    partial class Server
    {
        private void BeginNewGame()
        {
            Random rng = new Random();
            int playerOne = rng.Next(0, 2);

            if (playerOne == 1)
            {
                _clientList[0].PlayerID = Client.PlayerNumber.PlayerOne;
                _clientList[1].PlayerID = Client.PlayerNumber.PlayerTwo;
            }
            else
            {
                _clientList[1].PlayerID = Client.PlayerNumber.PlayerOne;
                _clientList[0].PlayerID = Client.PlayerNumber.PlayerTwo;
            }

            _Game = new CrusadeGame();

            Console.WriteLine("Game has started.");
            UpdateAllClients(GenerateResponse(ResponseTypes.ClientResponse, "GAMESTARTED"));
        }


        private void ShutdownGame()
        {
            _Game = null;
            lock(_clientList)
            {
                foreach (Client client in _clientList)
                    client.PlayerID = Client.PlayerNumber.NotAPlayer;
            }

            Console.WriteLine("Game has ended.");
            foreach (Client client in _clientList)
            {
                SendData(client, GenerateResponse(ResponseTypes.ClientResponse, "GAMEOVER"));
            }
        }


        private void UpdateAllClients(JSONResponse jsonResponse)
        {
            foreach (Client client in _clientList.ToArray())
                SendData(client, jsonResponse);
        }


        internal void GiveClientsBoardState()
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

            UpdateAllClients(GenerateResponse(ResponseTypes.GameResponse, sb.ToString()));
        }
    }
}
