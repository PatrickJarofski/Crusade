﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Deck : BaseGameObject
    {
        #region Members

        List<Card> _cardDeck;
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
            _cardDeck = new List<Card>();

            AddDebugCards();
        }


        private void AddDebugCards()
        {            
            AddCardToDeck(new TroopCard("Swordsman"));
            AddCardToDeck(new EquipCard("Longsword"));
            AddCardToDeck(new FieldCard("Second Wind"));
            AddCardToDeck(new TroopCard("Archer"));
            AddCardToDeck(new EquipCard("Composite Bow"));
            AddCardToDeck(new TroopCard("Knight"));
            AddCardToDeck(new FieldCard("Battle Standard"));
            AddCardToDeck(new TroopCard("Catapult"));
            AddCardToDeck(new EquipCard("Plate Armor"));
            AddCardToDeck(new TroopCard("Crusader"));
            ShuffleDeck();
        }


        /// <summary>
        /// Draws a card from the Deck         
        /// </summary>
        /// <returns>Returns the Card drawn.
        /// If the Deck has no cards left, returns null.</returns>
        public Card DrawCard()
        {
            if (_cardDeck.Count > 0)
            {
                Card temp = _cardDeck[0];
                _cardDeck.Remove(_cardDeck[0]);
                return temp;
            }

            else
                return null;
        }


        /// <summary>
        /// Randomizes the order of the cards
        /// in the Deck.
        /// </summary>
        public void ShuffleDeck()
        {           
            int n = _cardDeck.Count;
            while (n > 1)
            {
                n--;
                int k = CrusadeGame.RNG.Next(n + 1);
                Card value = _cardDeck[k];
                _cardDeck[k] = _cardDeck[n];
                _cardDeck[n] = value;
            }  
        }
        

        public void AddCardToDeck(Card card)
        {
            if (card == null)
                return;
            else
                _cardDeck.Add(card);
        }
        #endregion
    }
}
