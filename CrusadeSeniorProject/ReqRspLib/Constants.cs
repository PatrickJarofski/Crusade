using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public static class Constants
    {
        public const byte TYPE_TROOP = 0x01;
        public const byte TYPE_EQUIP = 0x02;
        public const byte TYPE_FIELD = 0x03;

        // NoLocation, Deck, Hand, Field, Grave

        public const byte LOCATION_NONE = 0x10;
        public const byte LOCATION_DECK = 0x11;
        public const byte LOCATION_HAND = 0x12;
        public const byte LOCATION_FIELD = 0x13;
        public const byte LOCATION_GRAVE = 0x14;
    }
}
