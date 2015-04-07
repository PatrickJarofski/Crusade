using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace CrusadeGameClient
{
    public class BoardScreen : GameScreen
    {
        private string cellPath;
        private string bgPath;
        private Texture2D cellImage;
        private Texture2D backgroundImage;

        private const int BOARD_WIDTH = 5;
        private const int BOARD_HEIGHT = 5;

        private int cellWidth;
        private int cellHeight;

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
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            DrawGameboard(spriteBatch);
        }


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


        public override void DrawHand(SpriteBatch spriteBatch, List<ReqRspLib.ClientCard> hand)
        {
            int yLocation = 350;
            for (int i = 0; i < hand.Count; ++i)
            {
                DrawCard(spriteBatch, hand[i].Name, i, yLocation);
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


        public override void DrawGamePieces(SpriteBatch spriteBatch, ReqRspLib.ClientGamePiece[,] board)
        {
            int width = board.GetUpperBound(0) + 1;
            int height = board.GetUpperBound(1) + 1;

            foreach(ReqRspLib.ClientGamePiece piece in board)
            {
                if(piece != null)
                {
                    DrawGamepiece(spriteBatch, piece.Name, piece.ColCoordinate, piece.RowCoordinate);
                }
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void CardClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Card was clicked.");
        }
    }
}
