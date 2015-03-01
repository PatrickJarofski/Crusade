using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGameFinished : IResponse
    {
        Guid _winner;

        public ResponseGameFinished(Guid winner)
        {
            _winner = winner;
        }

        public void Execute(ICrusadeClient client)
        {
            string msg;

            if (_winner == client.ID)
                msg = "Game is over. You win!";

            else if (_winner == Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"))
                msg = "Game is over. Tie game!";

            else
                msg = "Game is over. You lose...";

            Console.WriteLine("{0}====================================", Environment.NewLine);
            Console.WriteLine(msg);
            Console.WriteLine("===================================={0}", Environment.NewLine);
            client.EndGame();
        }
    }
}
