using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateAwaitAction : State
    {
        private string _name;
        public override string Name { get { return _name; } }

        public StateAwaitAction()
        {
            _name = "Await Action";
        }


        public override State entry(CrusadeGame game, object obj)
        {
            // stub
            return this;
        }


        public override ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot)
        {
            if (game.CurrentPlayer.ID != playerId)
                throw new IllegalActionException("It is not your turn.");
            else
                return game.CurrentPlayer.PlayCard(cardSlot);
        }



        public override ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot, int row, int col)
        {
            if (game.CurrentPlayer.ID != playerId)
                throw new IllegalActionException("It is not your turn.");
            else
            {
                game.Board.PlaceGamePiece(new TroopPiece(row, col));
                return game.CurrentPlayer.PlayCard(cardSlot);
            }
        }
    }
}
