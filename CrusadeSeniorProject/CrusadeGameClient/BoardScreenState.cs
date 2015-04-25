using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    internal abstract class BoardScreenState
    {
        protected Vector2 mousePos;

        protected MouseState currentMouseState;
        protected MouseState previousMouseState;

        protected bool mouseInRange(int min, int max, int mouse)
        {
            return mouse >= min && mouse <= max;
        }

        public virtual void LoadContent()
        {

        }


        public virtual void UnloadContent()
        {

        }


        public virtual BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            return this;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
