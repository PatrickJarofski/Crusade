using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public static class RequestTypes
    {
        public const byte BadRequest        = 0x00;
        public const byte ClientRequest     = 0x01;
        public const byte GameRequest       = 0x02;
        public const byte MessageRequest    = 0x03;        
    }
}
