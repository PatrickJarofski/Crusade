using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public static class ResponseTypes
    {
        public const byte BadResponse       = 0x00;
        public const byte ClientResponse    = 0x01;
        public const byte GameResponse      = 0x02;
        public const byte MessageResponse   = 0x03;
    }
}
