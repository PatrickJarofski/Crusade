using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    [Serializable]
    public class RequestTest : IRequest
    {
        public void Execute(Server server)
        {
            Console.WriteLine("Did it work?");
        }
    }
}
