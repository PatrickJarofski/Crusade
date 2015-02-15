using System;
using System.Collections.Generic;

namespace ReqRspLib
{
    public class CrusadeGamePiece
    {
        public string name;
        public Guid owner;
        public int row;
        public int col;
        public int attack;
        public int defense;
        public int minAttackRange;
        public int maxAttackRange;
        public int moveRange;

        public void print()
        {
            Console.WriteLine(name);
            Console.WriteLine("Owner: {0}", owner.ToString());
            Console.WriteLine("Location: Row {0}, Col {1}", row.ToString(), col.ToString());
            Console.WriteLine("Attack: {0}, Defense: {1}", attack.ToString(), defense.ToString());
            Console.WriteLine("Attack Range: {0} to {1}", minAttackRange.ToString(), maxAttackRange.ToString());
            Console.WriteLine("Movement: {0}", moveRange.ToString());
        }
    }
}
