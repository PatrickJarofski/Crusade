using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class TroopOptionState : BoardScreenState
    {
        Texture2D menuImage;
        Rectangle rec;

        readonly GameCell cell;
        readonly GameCell[,] board;

        bool notFirstCheck = false;

        const string path = "Gameboard/TroopOptionMenu.png";

        public TroopOptionState(GameCell selectedCell, GameCell[,] gameboard)
            :base()
        {
            this.cell = selectedCell;
            board = gameboard;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            menuImage = ScreenManager.Instance.Content.Load<Texture2D>(path);
            rec = new Rectangle(cell.X, cell.Y, menuImage.Width, menuImage.Height);
        }


        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            base.Update(gameTime, previous, current);

            if (mouseClick() && notFirstCheck)
                return handleMouseClick();

            if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) || !mouseInRange())
                return new AwaitUserInputState();

            if (!notFirstCheck)
                notFirstCheck = true;
            
            return this;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                base.Draw(spriteBatch);
                spriteBatch.Draw(menuImage, rec, Color.White);
            }
            catch(Exception ex)
            {
                ServerConnection.Instance.WriteError("TroopOptionState Error: " + ex.Message);
            }
        }


        private bool mouseInRange()
        {
            return mouseInRange(rec.Left, rec.Right, currentMouseState.X) && mouseInRange(rec.Top, rec.Bottom, currentMouseState.Y);
        }


        private bool mouseClick()
        {
            return (previousMouseState.LeftButton == ButtonState.Pressed) && (currentMouseState.LeftButton == ButtonState.Released);
        }


        private BoardScreenState handleMouseClick()
        {
            int mouseX = currentMouseState.X;
            int mouseY = currentMouseState.Y;

            // Clicked Move Troop
            if (mouseInRange(rec.Left, rec.Right, currentMouseState.X) && mouseInRange(rec.Top, rec.Top + (rec.Height / 2), currentMouseState.Y))
                return new MoveTroopState(cell, board);

            // Clicked Attack Troop
            if (mouseInRange(rec.Left, rec.Right, currentMouseState.X) && mouseInRange(rec.Top + (rec.Height / 2) + 1, rec.Bottom, currentMouseState.Y))
                return new AttackTroopState(cell, board);

            // Default
            return this;
        }
    }
}
