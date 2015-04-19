using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class GamepieceMenuState : BoardScreenState
    {
        readonly string attackRange;
        readonly string owner;

        GameCell _cell;
        Texture2D image;
        Rectangle rec;
        SpriteFont font;
        int mouseX;
        int mouseY;

        private int textX = 535;
        private int textY = 61;

        private int recX = 500;
        private int recY = 60;

        public GamepieceMenuState(GameCell cell)
            :base()
        {
            _cell = cell;
            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;
            mousePos = new Vector2(mouseX, mouseY);

            attackRange = _cell.GamepieceImg.Gamepiece.MinAttackRange.ToString() + "-" + _cell.GamepieceImg.Gamepiece.MaxAttackRange.ToString();
            if (_cell.GamepieceImg.Gamepiece.Owner == ServerConnection.Instance.ID.ToString())
                owner = "You";
            else
                owner = "Opponent";
        }

        public override void LoadContent()
        {
            base.LoadContent();
            image = ScreenManager.Instance.Content.Load<Texture2D>("Gameboard/Menu.png");
            font = ScreenManager.Instance.Content.Load<SpriteFont>("MonoFont");
            rec = new Rectangle(recX, recY, image.Width, image.Height);
        }

        // Draw the stat rectangle and gamepiece stats
        public override void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(image, rec, Color.White);
            Vector2 vec = new Vector2(textX, textY); 

            if(_cell.GamepieceImg != null)
                spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Name, vec, Color.Black);

            vec.Y += 12;
            spriteBatch.DrawString(font, owner, vec, Color.Black);

            vec.Y += 12;
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Attack.ToString(), vec, Color.Black);

            vec.Y += 12;
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Defense.ToString(), vec, Color.Black);

            vec.Y += 12;
            spriteBatch.DrawString(font, attackRange, vec, Color.Black);

            vec.Y += 12;
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Move.ToString(), vec, Color.Black);
            base.Draw(spriteBatch);            
        }


        public new GamepieceMenuState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (mouseInRange(_cell.X, _cell.X + image.Width, current.X) &&
                mouseInRange(_cell.Y, _cell.Y + image.Height, current.Y))
            {
                base.Update(gameTime, previous, current);
                return this;
            }

            else
                return null;            
        }
    }
}
