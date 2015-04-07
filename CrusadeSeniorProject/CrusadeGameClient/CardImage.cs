using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public class CardImage : CrusadeImage
    {     
        public CardImage(string imgPath, int xCoord, int yCoord)
            :base(imgPath, xCoord, yCoord)
        {

        }

        public override void Draw(ContentManager content, SpriteBatch spriteBatch)
        {
            if (path != null && path != String.Empty)
            {
                try
                {
                    int offset = ScreenManager.SCREEN_WIDTH / 8;

                    image = content.Load<Texture2D>(path);
                    Rectangle rec = new Rectangle((Col * image.Width) + offset, Row, image.Width, image.Height);
                    spriteBatch.Draw(image, rec, Color.White);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("CardImage Error: " + ex.Message);
                }
            }
        }
    }
}
