using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public class GameCell
    {
        public const string IMG_PATH = "Gameboard/Cell.png";

        private int row;
        private int col;
        private int x;
        private int y;
        private Texture2D image;
        private Rectangle rec;

        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public Texture2D Image { get { return image; } }
        public GamepieceImage GamepieceImg { get; set; }

        public GameCell(int row, int col)
        {
            image = ScreenManager.Instance.Content.Load<Texture2D>(IMG_PATH);
            this.row = row;
            this.col = col;

            x = col * image.Width + (ScreenManager.SCREEN_WIDTH / 4) - 10;
            y = row * image.Height;

            rec = new Rectangle(x, y, image.Width, image.Height);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, rec, Color.White);
            if(GamepieceImg != null)
                GamepieceImg.Draw(spriteBatch);
        }
    }
}
