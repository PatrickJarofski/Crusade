using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public enum CardLocation { NoLocation, Deck, Hand, Field, Grave };

    public enum CardType { Invalid, Troop, Equip, Field };

    public interface ICard
    {
        /// <summary>
        /// Gets the name of the Card.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the location of the card (Deck, Hand, Field, etc.)
        /// </summary>
        CardLocation Location { get; set; }

        /// <summary>
        /// Gets the type of card the object is (Troop, Equip, or Field).
        /// </summary>
        CardType Type { get; }

        // TODO
        // List<string> GetInformation();
    }
}
