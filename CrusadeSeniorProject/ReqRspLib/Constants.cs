using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public static class Constants
    {
        public const byte BAD_ATTRIBUTE = 0xF0;

        public const byte CARD_TYPE_TROOP = 0x01;
        public const byte CARD_TYPE_EQUIP = 0x02;
        public const byte CARD_TYPE_FIELD = 0x03;

        public const byte CARD_LOCATION_NONE = 0x10;
        public const byte CARD_LOCATION_DECK = 0x11;
        public const byte CARD_LOCATION_HAND = 0x12;
        public const byte CARD_LOCATION_FIELD = 0x13;
        public const byte CARD_LOCATION_GRAVE = 0x14;        
    }
}
