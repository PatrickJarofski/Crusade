using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateDrawCard : State
    {
        private string _name;

        public StateDrawCard()
        {
            _name = "Draw Card";
        }

        public override string Name { get { return _name; } }

        public override State entry(CrusadeGame game, object obj)
        {
            if (obj != null)
                throw new GameStateException("Invalid Action requested. Game is currently in a Draw Card state.");

            game.CurrentPlayer.DrawFromDeck();
            return new StateAwaitAction().entry(game, obj);
        }


    }
}
