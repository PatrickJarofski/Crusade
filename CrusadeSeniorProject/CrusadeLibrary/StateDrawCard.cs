using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateDrawCard : State
    {
        public override State performAction(CrusadeGame game, object obj)
        {
            if (obj != null)
                throw new GameStateException("Invalid Action requested. Game is currently in a Draw Card state.");

            // Draw card for _currentPlayer
            // return new StateAwaitAction();
            return new StateAwaitAction().performAction(game, obj);
        }
    }
}
