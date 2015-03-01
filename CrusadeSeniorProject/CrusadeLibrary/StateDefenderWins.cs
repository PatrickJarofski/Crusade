using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateDefenderWins : State
    {
        private string _name = "StateDefenderWins";

        public override string Name
        {
            get { return _name; }
        }

        public override State entry(CrusadeGame game, object obj)
        {
            return this;
        }

        public override Guid GetWinner(CrusadeGame game)
        {
            // Since the defending player won
            // we'll return the ID of whoever is NOT
            // the current player
            if (game.CurrentPlayer == game.Player1)
                return game.Player2.ID;
            else
                return game.Player1.ID;
        }
    }
}
