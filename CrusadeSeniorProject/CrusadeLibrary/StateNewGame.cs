using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateNewGame : State
    {
        private void DealStartingHand(Player player)
        {
            for (int i = 0; i < Hand.STARTING_HAND_SIZE; ++i)
                player.DrawFromDeck();
        }

        private void CreateDebugPieces(CrusadeGame game)
        {
            TroopPiece debug1 = new TroopPiece(0, 2);
            TroopPiece debug2 = new TroopPiece(4, 2);
            game._board.PlaceGamePiece(debug1);
            game._board.PlaceGamePiece(debug2);
        }

        public override State performAction(CrusadeGame game, object obj)
        {
            if (obj != null)
                throw new GameStateException("Invalid Action requested. Game is currently in a New Game state.");

            game.CurrentPlayer = game.Player1;

            DealStartingHand(game.Player1);
            DealStartingHand(game.Player2);

            CreateDebugPieces(game);

            return this;
        }
    }
}
