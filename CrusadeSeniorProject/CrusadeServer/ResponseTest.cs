using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    [Serializable]
    public class ResponseTest : IResponse
    {
        public void Execute()
        {
            Console.WriteLine("I have no idea what I'm doing.");
        }
    }
}
