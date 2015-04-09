﻿using System;
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

        public int Index { get { return _index; } }

        public CardImage(string imgPath, int xCoord, int yCoord, int index)
            :base(imgPath, xCoord, yCoord)
        {
            _index = index;
        }

        public override void Draw(ContentManager content, SpriteBatch spriteBatch)
        {
            if (path != null && path != String.Empty)
            {
                try
                {
                    int offset = ScreenManager.SCREEN_WIDTH / 8;

                    image = content.Load<Texture2D>(path);
                    rec = new Rectangle((xLoc * image.Width) + offset, yLoc, image.Width, image.Height);
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
