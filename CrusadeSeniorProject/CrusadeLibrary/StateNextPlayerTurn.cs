using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateNextPlayerTurn : State
    {
        private void setCurrentPlayer(CrusadeGame game)
        {
            if (game.CurrentPlayer == game.Player1)
                game.CurrentPlayer = game.Player2;
            else
                game.CurrentPlayer = game.Player1;
        }


        public override State performAction(CrusadeGame game, object obj)
        {
            if (obj != null)
                throw new GameStateException("Invalid Action requested. Game is currently in a Next Player Turn state.");

            setCurrentPlayer(game);
            game.CurrentPlayer.ActionPoints = Player.DEFAULT_ACTION_POINTS; // Replenish Action Points (AP)

            return new StateDrawCard().performAction(game, obj);
        }
    }
}
