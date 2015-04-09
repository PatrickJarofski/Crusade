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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CrusadeGameClient : Game
    {
        private readonly ServerConnection _serverConnection;

        GraphicsDeviceManager graphicsManager;
        SpriteBatch spriteBatch;

        private static CrusadeGameClient instance;

        public static CrusadeGameClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new CrusadeGameClient();

                return instance;
            }
        }
        
        private CrusadeGameClient()
            : base()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            _serverConnection = ServerConnection.Instance;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphicsManager.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            graphicsManager.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphicsManager.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);
            Window.Title = "Crusade Client";
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                lock (_serverConnection)
                {
                    _serverConnection.Disconnect();
                    Exit();
                }
            }

            ScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            ScreenManager.Instance.Draw(spriteBatch);

            if (_serverConnection.HandUpdated)
                ScreenManager.Instance.DrawHand(spriteBatch, _serverConnection.Hand);
            else
                ScreenManager.Instance.DrawHand(spriteBatch);

            if (_serverConnection.BoardUpdated)
                ScreenManager.Instance.DrawGamePieces(spriteBatch, _serverConnection.Gameboard);
            else
                ScreenManager.Instance.DrawGamePieces(spriteBatch);

            spriteBatch.End();
        }
     
    }
}
