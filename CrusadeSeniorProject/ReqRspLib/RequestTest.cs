using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestTest : IRequest
    {
        public void Execute(ICrusadeServer server)
        {
            Console.WriteLine("Did it work?");
        }
    }
}
