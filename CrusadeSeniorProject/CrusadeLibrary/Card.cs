using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public enum CardLocation { NoLocation, Deck, Hand, Field, Grave };

    public enum CardType { Troop, Equip, Field };

    public abstract class Card : BaseGameObject
    {
        private string _name;
        private CardType _type;

        #region Properties
        public CardLocation Location { get; set; }

        public CardType Type { get { return _type; } }

        public string Name
        {
            get { return _name; }
        }
        #endregion

        public Card(string name, CardType type)
        {
            _name = name;
            _type = type;
        }
    }
}
