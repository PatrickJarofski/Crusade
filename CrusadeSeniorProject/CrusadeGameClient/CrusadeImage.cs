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

        public event EventHandler ClickEvent;

        public int Col { get; set; }
        public int Row { get; set; }
        public Guid ID { get { return id; } }
        public bool IsSelected { get { return isSelected; } }


        public abstract void Draw(ContentManager content, SpriteBatch spriteBatch);

        public CrusadeImage(string path, int x, int y)
        {
            this.path = path;
            Col = x;
            Row = y;
            id = Guid.NewGuid();
        }
    }
}
