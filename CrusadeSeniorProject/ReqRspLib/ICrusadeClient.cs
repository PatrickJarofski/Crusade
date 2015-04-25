using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public interface ICrusadeClient
    {
        Guid ID { get; }

        int BackRow { get; set; }

        int DeckCount { get; set; }

        int ActionPoints { get; set; }

        bool IsTurnPlayer { get; set; }

        string GameOverMessage { get; set; }

        ConsoleColor PlayerColor { get; set; }       

        void SetHand(List<ClientCard> newHand);

        void SetGameboard(ClientGamePiece[,] board);

        void DisplayGameboard();

        void BeginGame();

        void EndGame();

        void BeginNextTurn(Guid turnPlayerId);

        void GetPlayerAction();

        void GetCardToPlay();

        void GetTroopToMove();
    }
}
