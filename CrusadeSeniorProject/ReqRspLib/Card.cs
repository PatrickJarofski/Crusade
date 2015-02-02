using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class Card : ICard
    {
        private string _name;
        private byte _type;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the Card.</param>
        /// <param name="type">Type of the Card (Troop, Equip, or Field).</param>
        public Card(string name, byte type)
        {
            _name = name;
            _type = type;
        }


        /// <summary>
        /// Gets the name of the Card.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the location of the card (Deck, Hand, Field, etc.)
        /// </summary>
        public byte Location { get; set; }

        /// <summary>
        /// Gets the type of card the object is (Troop, Equip, or Field).
        /// </summary>
        public byte Type { get { return _type; } }

        // TODO
        // public List<string> GetInformation();
    }
}
