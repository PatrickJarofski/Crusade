using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Card
    {
        private readonly string _name;
        // Have a GUID for unique id purposes?

        public string CardName { get { return _name; } }


        #region Methods

        public Card(string name)
        {
            _name = name;
        }

        public Card()
            : this("Default Name")
        {

        }

        #endregion
    }
}
