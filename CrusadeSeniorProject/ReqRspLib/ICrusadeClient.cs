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
        

        void SetHand(List<string> hand);

        void SetGameboard(string[,] board);

        void BeginGame();

        void EndGame();

        void BeginNextTurn(Guid turnPlayerID);

        void GetPlayerDecision();
    }
}
