using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class Responses
    {
        public const string GameOver = "GAME_OVER";
        public const string GameStarted = "GAME_STARTED";

        public const string GiveGameboard = "GIVE_GAMEBOARD\n";
        public const string GiveHand = "GIVE_HAND\n";
        public const string CardPlayed = "CARD_PLAYED\n";
    }
}
