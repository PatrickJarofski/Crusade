using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public abstract class CrusadeImage
    {
        private Guid id;

        protected Texture2D image;
        protected string path;
        protected bool isSelected = false;
        protected Rectangle rec;
        protected int xLoc;
        protected int yLoc;

        public Guid ID { get { return id; } }
        public bool IsSelected { get { return isSelected; } }
        public Rectangle Region { get { return rec; } }
        public Texture2D Image { get { return image; } }


        public abstract void Draw(SpriteBatch spriteBatch);

        public CrusadeImage(string path, int x, int y)
        {
            this.path = path;
            xLoc = x;
            yLoc = y;
            id = Guid.NewGuid();
            rec = new Rectangle();
        }

        public virtual void Select()
        {

        }

        public virtual void Deselect()
        {

        }
    }
}
