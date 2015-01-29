using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    class InvalidCard : Card, ICard
    {
        internal InvalidCard(string name)
            :base(name, CardType.Invalid)
        {

        }
    }
}
