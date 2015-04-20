using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class GamepieceMenuState : BoardScreenState
    {
        private readonly string attackRange;
        private readonly string owner;

        private readonly GameCell _cell;

        private readonly int textX = CrusadeGameClient.Instance.Window.ClientBounds.Right - 105;
        private readonly int textY = CrusadeGameClient.Instance.Window.ClientBounds.Top + 61;
        private readonly int recX = CrusadeGameClient.Instance.Window.ClientBounds.Right - 140;
        private readonly int recY = CrusadeGameClient.Instance.Window.ClientBounds.Top + 60;

        private Texture2D image;
        private Rectangle rec;
        private SpriteFont font;
        private int mouseX;
        private int mouseY;

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
            if (_cell.GamepieceImg != null)
            {
                spriteBatch.Draw(image, rec, Color.White);
                Vector2 vec = new Vector2(textX, textY);
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
            }

            base.Draw(spriteBatch);            
        }


        public new GamepieceMenuState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (mouseInRange(_cell.Region.Left, _cell.Region.Right, current.X) &&
                mouseInRange(_cell.Region.Top, _cell.Region.Bottom, current.Y))
            {
                base.Update(gameTime, previous, current);
                return this;
            }

            else
                return null;            
        }
    }
}
