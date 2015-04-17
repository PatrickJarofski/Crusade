using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    internal abstract class BoardScreenState
    {
        const string NORMAL_CURSOR_PATH = "Cursors/NormalCursor.png";
        const string VALID_CURSOR_PATH = "Cursors/ValidChoiceCursor.png";
        const string INVALID_CURSOR_PATH = "Cursors/InvalidChoiceCursor.png";

        protected Texture2D normalCursor;
        protected Texture2D validChoiceCursor;
        protected Texture2D invalidChoiceCursor;

        protected Texture2D cursorImage;
        protected Vector2 mousePos;

        protected MouseState currentMouseState;
        protected MouseState previousMouseState;

        protected bool mouseInRange(int min, int max, int mouse)
        {
            return mouse >= min && mouse <= max;
        }

        public virtual void LoadContent()
        {
            normalCursor = ScreenManager.Instance.Content.Load<Texture2D>(NORMAL_CURSOR_PATH);
            validChoiceCursor = ScreenManager.Instance.Content.Load<Texture2D>(VALID_CURSOR_PATH);
            invalidChoiceCursor = ScreenManager.Instance.Content.Load<Texture2D>(INVALID_CURSOR_PATH);
            cursorImage = normalCursor;
        }


        public virtual void UnloadContent()
        {
            validChoiceCursor.Dispose();
            invalidChoiceCursor.Dispose();
            normalCursor.Dispose();
            cursorImage = null;
        }


        public virtual BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            currentMouseState = current;
            previousMouseState = previous;
            mousePos = new Vector2(current.X, current.Y);            
            return this;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (cursorImage == null)
                LoadContent();

            if(mousePos.X < 10 && mousePos.Y < 10)
                spriteBatch.Draw(cursorImage, mousePos, Color.White);

            else
                spriteBatch.Draw(cursorImage, mousePos, Color.White);
        }


        public virtual void MoveTroop() { }

        public virtual void AttackTarget() { }
    }
}
