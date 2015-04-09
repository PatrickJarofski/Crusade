#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace CrusadeGameClient
{
    public class ScreenManager
    {
        public const int SCREEN_WIDTH = 640;
        public const int SCREEN_HEIGHT = 480;

        public Vector2 Dimensions { private set; get; }
        public ContentManager Content { private set; get; }

        private GameScreen currentScreen;
        private static ScreenManager instance;
        

        // Singleton
        private ScreenManager()
        {            
            currentScreen = new BoardScreen();
            Dimensions = new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT);
        }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();

                return instance;
            }
        }


        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent();
        }


        public void UnloadContent()
        {
            currentScreen.UnloadContent();
        }


        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);          
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }

        public void DrawHand(SpriteBatch spriteBatch)
        {
            currentScreen.DrawHand(spriteBatch);
        }

        public void DrawHand(SpriteBatch spriteBatch, List<ReqRspLib.ClientCard> hand)
        {
            currentScreen.DrawHand(spriteBatch, hand);
        }

        public void DrawGamePieces(SpriteBatch spriteBatch)
        {
            currentScreen.DrawGamePieces(spriteBatch);
        }

        public void DrawGamePieces(SpriteBatch spriteBatch, ReqRspLib.ClientGamePiece[,] board)
        {
            currentScreen.DrawGamePieces(spriteBatch, board);
        }

    }
}
