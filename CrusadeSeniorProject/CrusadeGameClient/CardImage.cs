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
        private int _index;
        private int xOffset = ScreenManager.SCREEN_WIDTH / 8;

        public int Index { get { return _index; } }

        public CardImage(string imgPath, int xCoord, int yCoord, int index)
            :base(imgPath, xCoord, yCoord)
        {
            image = ScreenManager.Instance.Content.Load<Texture2D>(path);            
            _index = index;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (path != null && path != String.Empty)
            {
                try
                {
                    rec = new Rectangle((xLoc * image.Width) + xOffset, yLoc, image.Width, image.Height);
                    spriteBatch.Draw(image, rec, Color.White);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("CardImage Error: " + ex.Message);
                }
            }
        }


        public override void Select()
        {
            isSelected = true;
            yLoc -= 10;
        }

        public override void Deselect()
        {
            isSelected = false;
            yLoc += 10;
        }
    }
}
