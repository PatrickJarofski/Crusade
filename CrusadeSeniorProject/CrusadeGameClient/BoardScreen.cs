using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace CrusadeGameClient
{
    public class BoardScreen : GameScreen
    {
        #region Fields
        private string cellPath;
        private string bgPath;
        private Texture2D cellImage;
        private Texture2D backgroundImage;

        private const int BOARD_WIDTH = 5;
        private const int BOARD_HEIGHT = 5;
        #endregion


        #region Public Methods

        public override void LoadContent()
        {
            base.LoadContent();
            cellPath = "Gameboard/Cell.png";
            bgPath = "Gameboard/Background.png";
            cellImage = content.Load<Texture2D>(cellPath);
            backgroundImage = content.Load<Texture2D>(bgPath);
        }

        public override void UnloadContent()
        {
            cellImage.Dispose();
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            handleMouseState();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            DrawGameboard(spriteBatch);
        }


        public override void DrawHand(SpriteBatch spriteBatch)
        {
            foreach (CardImage img in hand)
                img.Draw(content, spriteBatch);    
        }


        public override void DrawHand(SpriteBatch spriteBatch, List<ReqRspLib.ClientCard> newHand)
        {
            int yLocation = 360;
            hand.Clear();
            for (int i = 0; i < newHand.Count; ++i)
            {
                CardImage newImg = new CardImage("Cards/" + newHand[i].Name + ".png", i, yLocation);
                hand.Add(newImg);
            }

            DrawHand(spriteBatch);
        }      


        public override void DrawGamePieces(SpriteBatch spriteBatch)
        {
            foreach (GamepieceImage img in board)
                img.Draw(content, spriteBatch);
        }


        public override void DrawGamePieces(SpriteBatch spriteBatch, ReqRspLib.ClientGamePiece[,] newBoard)
        {
            board.Clear();
            foreach(ReqRspLib.ClientGamePiece piece in newBoard)
            {
                if (piece != null)
                {
                    string path = "Gameboard/" + piece.Name + ".png";
                    GamepieceImage img = new GamepieceImage(path, piece.ColCoordinate, piece.RowCoordinate);
                    board.Add(img);
                }
            }

            DrawGamePieces(spriteBatch);
        }             

        #endregion


        #region Private Methods

        private void DrawGameboard(SpriteBatch spriteBatch)
        {
            Microsoft.Xna.Framework.Rectangle rec = new Microsoft.Xna.Framework.Rectangle();
            rec.Width = cellImage.Width;
            rec.Height = cellImage.Height;

            Microsoft.Xna.Framework.Rectangle bgRec = new Microsoft.Xna.Framework.Rectangle(0, 0,
                ScreenManager.SCREEN_WIDTH, ScreenManager.SCREEN_HEIGHT);

            spriteBatch.Draw(backgroundImage, bgRec, Microsoft.Xna.Framework.Color.White);

            for (int i = 0; i < BOARD_WIDTH; ++i)
            {
                for (int j = 0; j < BOARD_HEIGHT; ++j)
                {
                    rec.X = i * cellImage.Width + (ScreenManager.SCREEN_WIDTH / 4) - 10;
                    rec.Y = j * cellImage.Height;
                    spriteBatch.Draw(cellImage, rec, Microsoft.Xna.Framework.Color.White);
                }
            }
        }


        private void DrawCard(SpriteBatch spriteBatch, string cardName, int x, int y)
        {
            try
            {
                string cardPath = "Cards/" + cardName + ".png";
                CardImage img = new CardImage(cardPath, x, y);
                img.Draw(content, spriteBatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DrawGamepiece(SpriteBatch spriteBatch, string pieceName, int x, int y)
        {
            try
            {
                string path = "Gameboard/" + pieceName + ".png";
                GamepieceImage piece = new GamepieceImage(path, x, y);
                piece.Draw(content, spriteBatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void handleMouseState()
        {
            checkMouseOnCard();

        }


        private void checkMouseOnCard()
        {
            foreach (CardImage card in Hand)
            {
                int xMin = card.Region.Left;
                int xMax = card.Region.Right;
                int yMin = card.Region.Top;
                int yMax = card.Region.Bottom;

                if (mouseWithinRange(xMin, xMax, currentMouseState.X) && mouseWithinRange(yMin, yMax, currentMouseState.Y))
                {
                    if (!card.IsSelected)
                    {
                        if (selectedCard != null)
                            selectedCard.Deselect();

                        selectedCard = card;
                        selectedCard.Select();
                    }
                    else
                    {
                        if (selectedCard != card)
                        {
                            card.Deselect();
                            selectedCard = null;
                        }
                    }
                }
            }
        }

        private bool mouseWithinRange(int min, int max, int mouseCoord)
        {
            return mouseCoord >= min && mouseCoord <= max;
        }
        #endregion
    }
}
