using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateNextPlayerTurn : State
    {
        private string _name;

        public override string Name { get { return _name; } }

        public StateNextPlayerTurn()
        {
            _name = "Next Turn";
        }

        private void setCurrentPlayer(CrusadeGame game)
        {
            if (game.CurrentPlayer == game.Player1)
                game.CurrentPlayer = game.Player2;
            else
                game.CurrentPlayer = game.Player1;
        }


        public override State entry(CrusadeGame game, object obj)
        {
            if (obj != null)
                throw new GameStateException("Invalid Action requested. Game is currently in a Next Player Turn state.");

            setCurrentPlayer(game);
            game.CurrentPlayer.ActionPoints = Player.DEFAULT_ACTION_POINTS; // Replenish Action Points (AP)

            return new StateDrawCard().entry(game, obj);
        }
    }
}
