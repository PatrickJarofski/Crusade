using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Player : BaseGameObject
    {
        public const int DEFAULT_ACTION_POINTS = 3;

        public enum PlayerNumber { PlayerOne, PlayerTwo, NotAPlayer };
        
        #region Fields
        private Deck _deck;
        private Hand _hand;

        #endregion


        #region Properties
        public int ActionPoints { get; set; }
        #endregion


        #region Methods

        /// <summary>
        /// Constructor for Player
        /// </summary>
        public Player()
        {
            _deck = new Deck();
            _hand = new Hand();
        }

        public Player(Guid id)
            :base(id)
        {
            _deck = new Deck();
            _hand = new Hand();
        }


        /// <summary>
        /// Draw a card from the deck and
        /// put that card in the hand
        /// </summary>
        public void DrawFromDeck()
        {
            Card cardDrawn = _deck.DrawCard();

            if (cardDrawn != null)            
                _hand.AddCard(cardDrawn);            
        }


        /// <summary>
        /// Randomize the order of the
        /// cards in the deck.
        /// </summary>
        public void ShuffleDeck()
        {
            _deck.ShuffleDeck();
        }


        /// <summary>
        /// Play a card from the hand.
        /// </summary>
        /// <param name="cardSlotInHand">The index in the collection where the card resides.</param>
        /// <returns></returns>
        public ICard PlayCard(int cardSlotInHand)
        {
            return _hand.RemoveCard(cardSlotInHand);
        }

        public List<Card> GetHand()
        {
            return _hand.GetHand();
        }

        #endregion

    }
}
