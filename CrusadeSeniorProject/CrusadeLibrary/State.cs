using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public abstract class State
    {
        public abstract string Name { get; }

        public abstract State entry(CrusadeGame game, object obj);

        public virtual State GetNextState(CrusadeGame game)
        {
            return this;
        }

        public virtual void BeginNextTurn()
        {
            throw new GameStateException("Invalid Action: Begin Next Turn. Game is currently in a " + Name + " state.");
        }

        public virtual ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot)
        {
            throw new GameStateException("Invalid Action: Play Card(player, slot). Game is currently in a " + Name + " state.");
        }

        public virtual ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot, int row, int col)
        {
            throw new GameStateException("Invalid Action: Play Card(player, slot, row, col). Game is currently in a " + Name + " state.");
        }

        public virtual void MoveTroop(CrusadeGame game, Guid ownerId, int startRow, int startCol, int endRow, int endCol)
        {
            throw new GameStateException("Invalid Action: Move Troop. Game is currently in a " + Name + " state.");
        }

        public virtual Tuple<State, List<string>> TroopCombat(CrusadeGame game, Guid turnPlayer, int atkRow, int atkCol, int defRow, int defCol)
        {
            throw new GameStateException("Invalid Action: TroopCombat(). Game is currently in a " + Name + " state.");
        }

        public virtual Guid GetWinner(CrusadeGame game)
        {
            return Guid.Empty;
        }
    }
}
