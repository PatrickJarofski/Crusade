using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    class CardInvalid : Card, ICard
    {
        internal CardInvalid(string name)
            :base(name, CardType.Invalid)
        {

        }
    }
}
