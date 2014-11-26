using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public enum CardLocation { NoLocation, Deck, Hand, Field, Grave };

    public interface Card : GameObject
    {
        CardLocation Location { get; set; }

        string Name { get; }
    }
}
