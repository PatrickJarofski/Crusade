using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeSeniorProject
{
    public class StateObject
    {
        public const int BufferSize = 1024;

        public Socket workSocket = null;
        public byte[] buffer = new byte[BufferSize];

        public void Clear()
        {
            buffer = new byte[BufferSize];
        }
    }
}
