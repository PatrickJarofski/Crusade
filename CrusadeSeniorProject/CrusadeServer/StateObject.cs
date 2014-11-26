using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class StateObject
    {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public System.Net.Sockets.Socket workerSocket;
        public StringBuilder sb = new StringBuilder();

        public void Clear()
        {
            buffer = new byte[BufferSize];
            sb.Clear();
        }

    }
}
