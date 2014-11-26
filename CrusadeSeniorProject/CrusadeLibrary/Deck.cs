using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Deck : BaseGameObject
    {
        #region Members

        Queue<Card> _cardDeck;
        #endregion

        #region Properties

        public int Count
        {
            get { return _cardDeck.Count; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor for Deck
        /// </summary>
        public Deck()
        {
            _cardDeck = new Queue<Card>();

            // Debug
            AddCardToDeck(new TroopCard("Swordsman"));
            AddCardToDeck(new EquipCard("Longsword"));
            AddCardToDeck(new FieldCard("Second Wind"));
        }


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
            _cardDeck.OrderBy(a => CrusadeGame.RNG.Next());  
        }


        public void AddCardToDeck(Card card)
        {
            if (card == null)
                return;
            else
                _cardDeck.Enqueue(card);
        }
        #endregion


        public override void Execute()
        {
            return;
        }
    }
}
