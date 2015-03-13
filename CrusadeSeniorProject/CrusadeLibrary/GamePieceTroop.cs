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
        public int RemainingDefense { get; set; }
        public int Move { get { return moveRange; } }
        public int MinAttackRange { get { return minAttackRange; } }
        public int MaxAttackRange { get { return maxAttackRange; } }

        public bool hasMoveRange(int sourceRow, int sourceCol, int destRow, int destCol)
        {
            int moveCost = Math.Abs(sourceRow - destRow) + Math.Abs(sourceCol - destCol);
            return Move >= moveCost;
        }


        public bool hasAttackRange(int sourceRow, int sourceCol, int destRow, int destCol)
        {
            int distance = Math.Abs(sourceRow - destRow) + Math.Abs(sourceCol - destCol);
            return MaxAttackRange >= distance && MinAttackRange <= distance;
        }


        public GamePieceTroop(int row, int col, Guid ownerId, string name)
            :base(row, col, GamePieceType.Troop, ownerId, name)
        {
            this.name = name;
            owner = ownerId;
            this.row = row;
            this.col = col;

            Tuple<int, int, int, int, int> stats = getStats(name);

            attack = stats.Item1;
            defense = stats.Item2;
            minAttackRange = stats.Item3;
            maxAttackRange = stats.Item4;
            moveRange = stats.Item5;
            resetRemainingDefense();
        }

        public void resetRemainingDefense()
        {
            RemainingDefense = defense;
        }

        private Tuple<int, int, int, int, int> getStats(string troopName)
        {
            int atk = 1;
            int def = 1;
            int minAtkRange = 1;
            int maxAtkRange = 1;
            int moveRange = 1;

            switch(troopName)
            {
                case "Commander":
                    atk = 7;
                    def = 9;
                    break;
                case "Archer":
                    atk = 3;
                    def = 3;
                    minAtkRange = 2;
                    maxAtkRange = 6;                    
                    break;
                case "Catapult":
                    atk = 6;
                    minAtkRange = 2;
                    maxAtkRange = 6;
                    break;
                case "Crossbowman":
                    atk = 3;
                    def = 3;
                    maxAtkRange = 6;
                    break;
                case "Crusader":
                    atk = 5;
                    def = 4;
                    break;
                case "Knight":
                    atk = 4;
                    def = 5;
                    moveRange = 2;
                    break;
                case "Juggernaut":
                    atk = 3;
                    def = 7;
                    break;
                case "Swordsman":
                    atk = 4;
                    def = 4;
                    break;
                default:
                    break;
            }

            return new Tuple<int, int, int, int, int>(atk, def, minAtkRange, maxAtkRange, moveRange);
        }
    }
}
