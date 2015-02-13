using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class IllegalActionException : Exception
    {
        public IllegalActionException(string error)
            :base(error)
        { }
    }
}
