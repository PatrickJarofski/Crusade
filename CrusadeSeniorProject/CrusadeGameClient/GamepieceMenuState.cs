﻿using System;
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


        public override void Draw(SpriteBatch spriteBatch)
        {            


            spriteBatch.Draw(image, rec, Color.White);
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Name, new Vector2(textX, textY), Color.Black);
            spriteBatch.DrawString(font, owner, new Vector2(textX, textY + 12), Color.Black);
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Attack.ToString(), new Vector2(textX, textY + 24), Color.Black);
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Defense.ToString(), new Vector2(textX, textY + 36), Color.Black);
            spriteBatch.DrawString(font, attackRange, new Vector2(textX, textY + 48), Color.Black);
            spriteBatch.DrawString(font, _cell.GamepieceImg.Gamepiece.Move.ToString(), new Vector2(textX, textY + 60), Color.Black);
            base.Draw(spriteBatch);            
        }


        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (mouseInRange(_cell.X, _cell.X + image.Width, current.X) &&
                mouseInRange(_cell.Y, _cell.Y + image.Height, current.Y))
                return base.Update(gameTime, previous, current);

            else
                return new AwaitUserInputState();
            
        }
    }
}
