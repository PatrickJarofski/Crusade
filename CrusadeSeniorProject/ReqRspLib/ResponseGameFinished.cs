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
                msg = "You win!";

            else if (_winner == Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"))
                msg = "Tie game.";

            else
                msg = "You lose...";

            client.GameOverMessage = msg;

            Console.WriteLine("{0}====================================", Environment.NewLine);
            Console.WriteLine(msg);
            Console.WriteLine("===================================={0}", Environment.NewLine);
            client.EndGame();
        }
    }
}
