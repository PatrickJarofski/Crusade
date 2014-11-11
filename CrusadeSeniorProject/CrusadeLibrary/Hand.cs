using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Hand
    {
        List<Card> _cardHand;

        /// <summary>
        /// Constructor for Hand
        /// </summary>
        public Hand()
        {
            _cardHand = new List<Card>();
        }


        /// <summary>
        /// Gets the cards currently in the Hand
        /// </summary>
        /// <returns>List of type Card</returns>
        public List<Card> GetHand()
        {
            return _cardHand;
        }


        /// <summary>
        /// Gets the number of cards in the Hand
        /// </summary>
        /// <returns>int equal to # of cards in the Hand</returns>
        public int Count()
        {
            return _cardHand.Count;
        }


        /// <summary>
        /// Add an acquired card to the hand
        /// </summary>
        /// <param name="card">Card to add</param>
        public void AddCard(Card card)
        {
            _cardHand.Add(card);
        }


        /// <summary>
        /// Gets the card the caller has
        /// chosen to play
        /// </summary>
        /// <param name="cardToPlay">Name of the card to play</param>
        /// <returns>A Card whose name matches
        /// the input parameter string</returns>
        public Card PlayCard(string cardToPlay)
        {
            return _cardHand.Find(a => a.CardName == cardToPlay);
        }


        /// <summary>
        /// Removes a specific card from the hand
        /// </summary>
        /// <param name="cardToRemove">Name of the card to remove</param>
        public void RemoveCard(string cardToRemove)
        {
            _cardHand.Remove(_cardHand.Find(a => a.CardName == cardToRemove));
        }


        /// <summary>
        /// Determines if the hand has a
        /// specific card
        /// </summary>
        /// <param name="cardToFind">name of the card to find</param>
        /// <returns>True or false depending on if the hand has the card</returns>
        public bool HasCard(string cardToFind)
        {
            return _cardHand.Exists(a => a.CardName == cardToFind);
        }


        /// <summary>
        /// The cards to be discarded are chosen
        /// randomly and removed from the Hand.
        /// It is up to the caller to deal with
        /// the list of cards returned.
        /// </summary>
        /// <param name="numCards">Number of cards to discard</param>
        /// <returns>List of type Card</returns>
        public List<Card> DiscardCards(int numCards)
        {
            List<Card> discardList = new List<Card>();

            // Shuffle the hand so the
            // cards are chosen randomly
            Random random = new Random();
            _cardHand.OrderBy(a => random.Next());

            for (int i = 0; i < numCards; ++i)
            {
                discardList.Add(_cardHand[i]);
                _cardHand.Remove(_cardHand[i]);
            }
            return discardList;
        }


    }
}
