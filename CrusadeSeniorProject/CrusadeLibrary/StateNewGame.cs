using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateNewGame : State
    {
        private string _name;

        public override string Name { get { return _name; } }

        public StateNewGame()
        {
            _name = "New Game";
        }

        private void DealStartingHand(Player player)
        {
            for (int i = 0; i < Hand.STARTING_HAND_SIZE; ++i)
                player.DrawFromDeck();
        }

        private void PlaceCommanders(CrusadeGame game)
        {
            GamePieceTroop commander1 = new GamePieceTroop(0, 2, game.Player1.ID, GamePiece.COMMANDER);
            GamePieceTroop commander2 = new GamePieceTroop(4, 2, game.Player2.ID, GamePiece.COMMANDER);
            game.Board.DeployGamePiece(commander1);
            game.Board.DeployGamePiece(commander2);
        }

        public override State entry(CrusadeGame game, object obj)
        {
            if (obj != null)
                throw new GameStateException("Invalid Action requested. Game is currently in a New Game state.");

            game.CurrentPlayer = game.Player2; // Next state will correct this to player 1
                                               // Necessary since NextPlayerTurn state flips
                                               // whoever the current player is

            DealStartingHand(game.Player1);
            DealStartingHand(game.Player2);

            PlaceCommanders(game);

            return new StateNextPlayerTurn().entry(game, null);
        }
    }
}
