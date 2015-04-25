using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public class GameDoneScreen : GameScreen
    {
        Texture2D background;
        Texture2D playAgainButton;

        Rectangle bgRec;
        Rectangle playAgainRec;

        SpriteFont font;
        Vector2 location;
        
        public GameDoneScreen() : base()
        {
            LoadContent();
        }

        
        public override void LoadContent()
        {
            base.LoadContent();
            background = ScreenManager.Instance.Content.Load<Texture2D>("GameDone/Background.png");
            playAgainButton = ScreenManager.Instance.Content.Load<Texture2D>("GameDone/PlayAgain.png");
            font = ScreenManager.Instance.Content.Load<SpriteFont>("MessageFont");

            bgRec = new Rectangle(0, 0, ScreenManager.SCREEN_WIDTH, ScreenManager.SCREEN_HEIGHT);
            playAgainRec = new Rectangle(240, 300, playAgainButton.Width, playAgainButton.Height);
            location = new Vector2(200, 200);

            contentLoaded = true;
        }


        public override void UnloadContent()
        {
            background.Dispose();
            playAgainButton.Dispose();
            base.UnloadContent();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseInRange(playAgainRec.Left, playAgainRec.Right, currentMouseState.X) &&
                    mouseInRange(playAgainRec.Top, playAgainRec.Bottom, currentMouseState.Y))
                    ServerConnection.Instance.RestartGame();
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bgRec, Color.White);
            spriteBatch.Draw(playAgainButton, playAgainRec, Color.White);
            spriteBatch.DrawString(font, ServerConnection.Instance.GameOverMessage, location, Color.White);
        }


        private bool mouseInRange(int min, int max, int mouse)
        {
            return mouse >= min && mouse <= max;
        }
    }
}
