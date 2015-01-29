using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class FieldCard : Card
    {
        #region Methods
        public FieldCard(string name)
            : base(name, CardType.Field)
        {
           
        }
        #endregion


    }
}
