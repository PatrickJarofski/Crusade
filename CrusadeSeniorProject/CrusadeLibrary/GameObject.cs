using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public interface GameObject
    {
        Guid ID { get; }

        void Execute();
    }
}
