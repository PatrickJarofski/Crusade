using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    class Program
    {
        public static void Main()
        {
            Server server = new Server();

            Console.WriteLine("Press Enter to terminate.");
            Console.Read();
        }        
    }
}
