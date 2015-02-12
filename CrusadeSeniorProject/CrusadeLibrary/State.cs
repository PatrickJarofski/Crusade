using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public abstract class State
    {
        public abstract State performAction(CrusadeGame game, object obj);
    }
}
