using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class EmptyGameObject : GameObject
    {
        public Guid ID
        {
            get { return Guid.Empty; }
        }

        public string Name
        {
            get { return String.Empty; }
        }

        public void Execute()
        {
            return;
        }
    }
}
