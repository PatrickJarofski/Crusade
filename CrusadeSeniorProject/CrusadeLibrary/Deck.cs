using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Deck
    {
        Queue<Card> _cardDeck;  


        /// <summary>
        /// Constructor for Deck
        /// </summary>
        public Deck()
        {
            _cardDeck = new Queue<Card>();     

            // Populate deck with player's cards
        }


        #region Methods

        /// <summary>
        /// Draws a card from the Deck         
        /// </summary>
        /// <returns>Returns the Card drawn.
        /// If the Deck has no cards left, returns null.</returns>
        public Card DrawCard()
        {
            if (_cardDeck.Count > 0)
                return _cardDeck.Dequeue();

            else
                return null;
        }


        /// <summary>
        /// Randomizes the order of the cards
        /// in the Deck.
        /// </summary>
        public void ShuffleDeck()
        {
            Random random = new Random();
            _cardDeck.OrderBy(a => random.Next());  
        }


        /// <summary>
        /// Gets the number of cards left in the deck
        /// </summary>
        /// <returns>int equal to the number of cards left</returns>
        public int GetCardCount()
        {
            return _cardDeck.Count;
        }
        #endregion

    }
}
