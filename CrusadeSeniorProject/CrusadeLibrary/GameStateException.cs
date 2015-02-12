using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class GameStateException : FormatException
    {
        public GameStateException(string error)
            :base(error)
        {
            
        }

        public GameStateException(string error, Exception innerException)
            : base(error, innerException)
        {

        }
    }
}
