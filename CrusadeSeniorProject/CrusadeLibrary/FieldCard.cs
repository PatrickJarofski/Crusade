using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class FieldCard : Card
    {
        private readonly Guid _guid;
        private readonly string _name;
        private CardLocation _cardLocation;


        #region Properties
        public Guid ID
        {
            get { return _guid; }
        }

        public string Name
        {
            get { return _name; }
        }

        public CardLocation Location
        {
            get { return _cardLocation; }
            set { _cardLocation = value; }
        }
        #endregion


        #region Methods
        public FieldCard(string name)
        {
            _guid = new Guid();
            _name = name;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
