using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class Card
    {
        public string Name;
        public string Type;
        public string Location;
        public int Attack;
        public int Defense;
        public int MinAttackRange;
        public int MaxAttackRange;
        public int MoveRange;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the Card.</param>
        /// <param name="type">Type of the Card (Troop, Equip, or Field).</param>
        public Card(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
