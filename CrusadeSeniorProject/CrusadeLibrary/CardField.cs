using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class CardField : Card, ICard
    {
        #region Methods
        public CardField(string name)
            : base(name, CardType.Field)
        {
           
        }
        #endregion
    }
}
