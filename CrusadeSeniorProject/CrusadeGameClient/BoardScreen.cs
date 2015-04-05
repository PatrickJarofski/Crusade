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
            Microsoft.Xna.Framework.Rectangle rec = new Microsoft.Xna.Framework.Rectangle();
            rec.Width = cellImage.Width;
            rec.Height = cellImage.Height;

            Microsoft.Xna.Framework.Rectangle bgRec = new Microsoft.Xna.Framework.Rectangle(0, 0, 
                ScreenManager.SCREEN_WIDTH, ScreenManager.SCREEN_HEIGHT);

            spriteBatch.Draw(backgroundImage, bgRec, Microsoft.Xna.Framework.Color.White);

            for(int i = 0; i < BOARD_WIDTH; ++i)
                for(int j = 0; j < BOARD_HEIGHT; ++j)
                {
                    rec.X = i * cellImage.Width + (ScreenManager.SCREEN_WIDTH / 4) - 10;
                    rec.Y = j * cellImage.Height;
                    spriteBatch.Draw(cellImage, rec, Microsoft.Xna.Framework.Color.White);
                }
        }
    }
}
