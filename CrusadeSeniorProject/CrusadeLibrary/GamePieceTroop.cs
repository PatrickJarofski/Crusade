using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class GamePieceTroop : GamePiece
    {
        string name;
        Guid owner;
        int row;
        int col;
        int attack;
        int defense;
        int minAttackRange;
        int maxAttackRange;
        int moveRange;

        public int Attack { get { return attack; } }
        public int Defense { get { return defense; } }
        public int Move { get { return moveRange; } }
        public int MinAttackRange { get { return minAttackRange; } }
        public int MaxAttackRange { get { return maxAttackRange; } }

        public bool hasEnoughMovement(int sourceRow, int sourceCol, int destRow, int destCol)
        {
            int moveCost = Math.Abs(sourceRow - destRow) + Math.Abs(sourceCol - destCol);
            return Move >= moveCost;
        }

        public GamePieceTroop(int row, int col, Guid ownerId, string name)
            :base(row, col, GamePieceType.Troop, ownerId, name)
        {
            this.name = name;
            owner = ownerId;
            this.row = row;
            this.col = col;
            attack = 1;
            defense = 1;
            minAttackRange = 1;
            maxAttackRange = 2;
            moveRange = 2;
        }
    }
}
