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
                throw new NotImplementedException("Non-Troop cards are currently not supported.");
        }


        public override ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot, int row, int col)
        {
            if (game.CurrentPlayer.ID != playerId)
                throw new IllegalActionException("It is not your turn.");
            else
            {
                try
                {
                    List<Card> hand = game.CurrentPlayer.GetHand();
                    if (hand[cardSlot].Type != CardType.Troop)
                        throw new NotImplementedException("Non-Troop cards are currently not supported.");

                    game.Board.PlaceGamePiece(new TroopPiece(row, col, playerId, hand[cardSlot].Name));

                    --game.CurrentPlayer.ActionPoints;

                    return game.CurrentPlayer.PlayCard(cardSlot);
                }
                catch(IndexOutOfRangeException)
                {
                    throw new IllegalActionException("Game does not recognize card chosen (Out of range).");
                }
                catch(ArgumentOutOfRangeException)
                {
                    throw new IllegalActionException("Game does not recognize card chosen (Out of range).");
                }
            }
        }

        public override State GetNextState(CrusadeGame game)
        {
            return new StateNextPlayerTurn().entry(game, null);
        }
    }
}
