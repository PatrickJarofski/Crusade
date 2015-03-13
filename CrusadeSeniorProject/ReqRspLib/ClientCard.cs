using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ClientCard
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MinAttackRange { get; set; }
        public int MaxAttackRange { get; set; }
        public int MoveRange { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the Card.</param>
        /// <param name="type">Type of the Card (Troop, Equip, or Field).</param>
        public ClientCard()
        {
            Name = ClientGamePiece.DEFAULT_STRING_VALUE;
            Owner = ClientGamePiece.DEFAULT_STRING_VALUE;
            Type = ClientGamePiece.DEFAULT_STRING_VALUE;
            Location = ClientGamePiece.DEFAULT_STRING_VALUE;
        }
    }
}
