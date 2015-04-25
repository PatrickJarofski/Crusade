using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class AwaitUserInputState : BoardScreenState
    {
        private Rectangle passTurnRec;
        private Texture2D passTurn;

        public override void LoadContent()
        {
            base.LoadContent();
            passTurn = ScreenManager.Instance.Content.Load<Texture2D>("Gameboard/PassTurn.png");
            passTurnRec = new Rectangle(ScreenManager.SCREEN_WIDTH - passTurn.Width - 10, 10, passTurn.Width, passTurn.Height);
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (CrusadeGameClient.Instance.Cursor != CrusadeGameClient.Instance.NormalCursor)
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.NormalCursor;

            if (previous.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released)
                checkPassTurn();

            return base.Update(gameTime, previous, current);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (passTurn == null || passTurnRec == null)
                LoadContent();

            spriteBatch.Draw(passTurn, passTurnRec, Color.White);
        }


        private void checkPassTurn()
        {
            if (mouseInRange(passTurnRec.Left, passTurnRec.Right, currentMouseState.X) &&
                mouseInRange(passTurnRec.Top, passTurnRec.Bottom, currentMouseState.Y))
                ServerConnection.Instance.PassTurn();
        }
        
    }
}
