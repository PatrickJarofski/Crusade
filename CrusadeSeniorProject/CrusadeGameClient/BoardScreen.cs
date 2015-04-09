using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace CrusadeGameClient
{
    public class BoardScreen : GameScreen
    {
        const int CARD_Y_LOC = 360;

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
            cellPath = null;
            bgPath = null;
            cellImage.Dispose();
            backgroundImage.Dispose();
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            handleMouseState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                handleMouseClick();
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
            hand.Clear();
            for (int i = 0; i < newHand.Count; ++i)
            {
                CardImage newImg = new CardImage("Cards/" + newHand[i].Name + ".png", i, CARD_Y_LOC, newHand[i].Index);
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


        private void handleMouseState()
        {
            checkMouseOnCard();
        }


        private void handleMouseClick()
        {
            CrusadeImage img = getImage();
            if (img != null)
            {
                Console.WriteLine("Got image at ({0},{1})", currentMouseState.X, currentMouseState.Y);
                if (img is CardImage)
                    Console.WriteLine("Index: {0}", (img as CardImage).Index);
            }
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
                    if(selectedCard != card)
                    {
                        if(selectedCard != null)                        
                            selectedCard.Deselect(); // Deselect previous card

                        selectedCard = card;
                        selectedCard.Select();                        
                    }
                }
            }

            if (hand.Count > 0)
            {
                int xMin = hand[0].Region.Left;
                int xMax = hand[hand.Count - 1].Region.Right;
                int yMax = CARD_Y_LOC + hand[0].Image.Height;

                if(!mouseWithinRange(xMin, xMax, currentMouseState.X) || !mouseWithinRange(CARD_Y_LOC, yMax, currentMouseState.Y))                    
                {
                    if(selectedCard != null)
                    {
                        selectedCard.Deselect();
                        selectedCard = null;
                    }
                }
            }
        }


        private bool mouseWithinRange(int min, int max, int mouseCoord)
        {
            return mouseCoord >= min && mouseCoord <= max;
        }


        private CrusadeImage getImage()
        {
            CrusadeImage imageToReturn = null;

            foreach(CardImage img in hand)
                if(mouseWithinRange(img.Region.Left, img.Region.Right, currentMouseState.X) && 
                    mouseWithinRange(img.Region.Top, img.Region.Bottom, currentMouseState.Y))
                {
                    imageToReturn = img;
                    return imageToReturn;
                }

            foreach(GamepieceImage img in board)
                if(mouseWithinRange(img.Region.Left, img.Region.Right, currentMouseState.X) &&
                    mouseWithinRange(img.Region.Top, img.Region.Bottom, currentMouseState.Y))
                    imageToReturn = img;

            return imageToReturn;
        }
        #endregion
    }
}
