using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CrusadeLibrary;

namespace SystemTesting
{
    [TestClass]
    public class CrusadeGameTest
    {
        Guid p1 = Guid.NewGuid();
        Guid p2 = Guid.NewGuid();

        [TestMethod]
        public void Constructor()
        {
            CrusadeGame game = new CrusadeGame(p1, p2);

            Assert.AreEqual(p1, game.CurrentPlayerId);
            Assert.AreEqual(3, game.CurrentPlayerAP);

            game.PassTurn(p1);
            Assert.AreEqual(p2, game.CurrentPlayerId);
            Assert.AreEqual(3, game.CurrentPlayerAP);           
        }

        [TestMethod]
        public void TestDeck()
        {
            CrusadeGame game = new CrusadeGame(p1, p2);

            Assert.AreEqual(4, game.GetDeckSize(p1));
            Assert.AreEqual(5, game.GetDeckSize(p2));
        }

        [TestMethod]
        public void TestBoardState()
        {
            CrusadeGame game = new CrusadeGame(p1, p2);

            GamePieceTroop[,] board = game.GetBoardState();

            Assert.AreEqual(5, board.GetLength(0));
            Assert.AreEqual(5, board.GetLength(1));

            int counter = 0;
            foreach (GamePieceTroop troop in board)
                if (troop != null)
                    ++counter;

            Assert.AreEqual(2, counter); // Should only be 2 pieces on the board to start: two commanders

            GamePieceTroop commander1 = new GamePieceTroop(0, 2, p1, "Commander");
            GamePieceTroop commander2 = new GamePieceTroop(4, 2, p2, "Commander");

            Assert.AreEqual(commander1.Owner, p1);
            Assert.AreEqual(commander2.Owner, p2);
            Assert.AreEqual(commander1.RowCoordinate, board[0, 2].RowCoordinate); 
            Assert.AreEqual(commander1.ColCoordinate, board[0, 2].ColCoordinate);

            Assert.AreEqual(commander2.RowCoordinate, board[4, 2].RowCoordinate);
            Assert.AreEqual(commander2.ColCoordinate, board[4, 2].ColCoordinate);

            Assert.AreEqual(commander1.Name, board[0, 2].Name);
            Assert.AreEqual(commander2.Name, board[4, 2].Name);
        }

        [TestMethod]
        public void TestPlayerColor()
        {
            CrusadeGame game = new CrusadeGame(p1, p2);

            Assert.AreEqual(ConsoleColor.Red, game.GetPlayerColor(p1));
            Assert.AreEqual(ConsoleColor.Blue, game.GetPlayerColor(p2));
        }
        
        [TestMethod]
        public void TestPlayerHand()
        {
            CrusadeGame game = new CrusadeGame(p1, p2);
            
            Assert.AreEqual(5, game.GetPlayerHand(Player.PlayerNumber.PlayerOne).Count);
            Assert.AreEqual(4, game.GetPlayerHand(Player.PlayerNumber.PlayerTwo).Count);
        }

        [TestMethod]
        public void TestMoveTroop()
        {
            CrusadeGame game = new CrusadeGame(p1, p2);

            game.MoveTroop(p1, 0, 2, 1, 2);
            GamePieceTroop[,] board = game.GetBoardState();

            Assert.AreEqual(null, board[0, 2]);
            Assert.AreEqual("Commander", board[1, 2].Name);

            game.MoveTroop(p1, 1, 2, 0, 2);
            board = game.GetBoardState();

            Assert.AreEqual(null, board[1, 2]);
            Assert.AreEqual("Commander", board[0, 2].Name);
            
        }

    }
}
