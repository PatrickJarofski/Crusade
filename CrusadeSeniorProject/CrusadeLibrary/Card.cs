using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public enum CardLocation { NoLocation, Deck, Hand, Field, Grave };

    public abstract class Card : BaseGameObject
    {
        private string _name;
        private CardLocation _cardLocation;

        #region Properties
        public CardLocation Location
        {
            get { return _cardLocation; }
            set { _cardLocation = value; }
        }

        public string Name
        {
            get { return _name; }
        }
        #endregion

        public Card(string name)
        {
            _name = name;
        }
    }
}
