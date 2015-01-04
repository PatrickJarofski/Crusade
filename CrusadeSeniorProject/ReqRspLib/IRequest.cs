using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    /// <summary>
    /// Interface all requests must implement.
    /// In addition all Requests need to have the [Serializable] tag
    /// </summary>
    public interface IRequest    
    {
        void Execute(Server server);
    }
}
