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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D sprite;
        Texture2D background;
        SpriteFont font;
        Rectangle rect;

        public CrusadeGameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);            
            _serverConnection = new ServerConnection();

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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";
            Window.Title = "Crusade Client";
            try
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                font = Content.Load<SpriteFont>("MonoFont");
                sprite = Content.Load<Texture2D>("Desert.jpg");
                background = Content.Load<Texture2D>("stars.jpg");

                rect = new Rectangle(0, 0, sprite.Width, sprite.Height);
                graphics.PreferredBackBufferHeight = 400;
                graphics.PreferredBackBufferWidth = 400;
            }
            catch(ContentLoadException ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                spriteBatch.Begin();
                DrawThing();
                spriteBatch.End();

                base.Draw(gameTime);
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Tried to load graphics that didn't exist.");
            }
        }

        private void DrawThing()
        {
            int x = Window.ClientBounds.Width / 2;
            int y = Window.ClientBounds.Height / 2;

            if(_serverConnection.InAGame)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

                Vector2 origin = new Vector2(sprite.Bounds.Location.X, sprite.Bounds.Location.Y);
                Vector2 location = new Vector2(x, y);

                spriteBatch.Draw(sprite, rect, Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Waiting for game to start...", new Vector2(x, y), Color.White);
            }
        }
    }
}
