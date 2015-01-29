using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public interface GameObject
    {
        /// <summary>
        /// Gets the unique ID of the object.
        /// </summary>
        Guid ID { get; }
    }

    public abstract class BaseGameObject : GameObject
    {
        private Guid _guid;

        /// <summary>
        /// Gets the unique ID of the object.
        /// </summary>
        public Guid ID
        {
            get { return _guid; }
        }

        public BaseGameObject()
        {
            _guid = Guid.NewGuid();
        }
    }
}
