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
        bool IsTurnPlayer { get; set; }

        List<ICard> Hand { get; set; }

        void SetGameboard(string[,] board);

        void BeginGame();

        void EndGame();

        void BeginNextTurn();

        void GetPlayerDecision();
    }
}
