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

        bool IsTurnPlayer { get; set; }

       // List<ClientCard> Hand { get; set; }

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
