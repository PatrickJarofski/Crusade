using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public static class Constants
    {
        public const char ResponseDelimiter = '%';
        public const char GameResponseDelimiter = '$';
        public static readonly char[] ResponseDelimiters = { ResponseDelimiter };
        public static readonly char[] GameResponseDelimiters = { GameResponseDelimiter };
    }
}
