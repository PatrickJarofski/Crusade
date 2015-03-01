using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateAttackerWins : State
    {
        private string _name = "StateAttackerWins";

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
            return game.CurrentPlayer.ID;
        }
    }
}
