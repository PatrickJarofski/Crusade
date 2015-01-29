using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public abstract class Card : BaseGameObject, ICard
    {
        private string _name;
        private CardType _type;

        #region Properties

        /// <summary>
        /// Gets the location of the card (Deck, Hand, Field, etc.)
        /// </summary>
        public CardLocation Location { get; set; }

        /// <summary>
        /// Gets the type of card the object is.
        /// </summary>
        public CardType Type { get { return _type; } }

        /// <summary>
        /// Gets the name of the card.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        #endregion

        /// <summary>
        /// Abstract Card object that all cards should inherit from.
        /// </summary>
        /// <param name="name">Name of the card</param>
        /// <param name="type">What type of card it is (Troop, Equip, or Field).</param>
        public Card(string name, CardType type)
        {
            _name = name;
            _type = type;
        }
    }
}
