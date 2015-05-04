using System;
using CrusadeLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemTesting
{
    [TestClass]
    public class CardTroopTest
    {
        [TestMethod]
        public void Constructor()
        {
            CardTroop card = new CardTroop("Commander");

            Assert.AreEqual("Commander", card.Name);
            Assert.AreEqual(CardType.Troop, card.Type);
            Assert.AreEqual(0, card.Index);
            Assert.AreEqual(CardLocation.NoLocation, card.Location);

            Assert.AreEqual(1, card.Attack);
            Assert.AreEqual(1, card.Defense);
            Assert.AreEqual(1, card.MinAttackRange);
            Assert.AreEqual(1, card.MaxAttackRange);
            Assert.AreEqual(1, card.MoveRange);
        }

        [TestMethod]
        public void ChangeLocation()
        {
            Card card = new CardTroop("Commander");

            card.Location = CardLocation.Deck;
            Assert.AreEqual(CardLocation.Deck, card.Location);

            card.Location = CardLocation.Field;
            Assert.AreEqual(CardLocation.Field, card.Location);

            card.Location = CardLocation.Grave;
            Assert.AreEqual(CardLocation.Grave, card.Location);

            card.Location = CardLocation.Hand;
            Assert.AreEqual(CardLocation.Hand, card.Location);

            card.Location = CardLocation.NoLocation;
            Assert.AreEqual(CardLocation.NoLocation, card.Location);
        }
    }
}
