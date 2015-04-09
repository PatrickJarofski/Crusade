using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public class GamepieceImage : CrusadeImage
    {
        /*
         * Gamepiece stats here
         */ 

        public GamepieceImage(string path, int x, int y)
            :base(path, x, y)
        {

        }


        public override void Draw(ContentManager content, SpriteBatch spriteBatch)
        {
            if(path != null && path != String.Empty)
            {
                try
                {
                    image = content.Load<Texture2D>(path);

                    int x = xLoc * 68 + 155;
                    int y = yLoc * 68 + 5;
                    rec = new Rectangle(x, y, image.Width, image.Height);
                    spriteBatch.Draw(image, rec, Color.White);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("GamepieceImage Error: " + ex.Message);
                }
            }
        }
    }
}
