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


        #region Methods
        public FieldCard(string name)
            : base(name)
        {

        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
