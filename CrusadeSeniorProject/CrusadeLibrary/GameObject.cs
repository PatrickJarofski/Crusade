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

    public abstract class BaseGameObject : GameObject
    {
        private Guid _guid;

        public Guid ID
        {
            get { return _guid; }
        }

        public BaseGameObject()
        {
            _guid = Guid.NewGuid();
        }


        abstract public void Execute();
    }
}
