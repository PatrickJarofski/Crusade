using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Player : BaseGameObject
    {
        public enum PlayerNumber { PlayerOne, PlayerTwo, NotAPlayer };
        
        #region Fields
        private Deck _deck;
        private Hand _hand;

        private int _actionPoints = 0;

        #endregion


        #region Properties
        public int ActionPoints { get { return _actionPoints; } }
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
        public string PlayCard(int cardSlotInHand)
        {
            return _hand.RemoveCard(cardSlotInHand);
        }


        public override void Execute()
        {
            return;
        }


        public List<string> GetHand()
        {
            return _hand.GetHand();
        }

        #endregion

    }
}
