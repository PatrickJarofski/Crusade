#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace CrusadeSeniorProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CrusadeGameClient : Game
    {
        ManualResetEvent sendDone = new ManualResetEvent(false);

        private readonly ServerConnection _Connection;

        private volatile bool exiting = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont MonoFont;

        private string _serverMessage = "No new message.";
        static Random random = new Random();


        private static System.Timers.Timer DEBUG_TIMER;

        public CrusadeGameClient()
            : base()
        {
            _Connection = new ServerConnection(this);

            DEBUG_TIMER = new System.Timers.Timer(2000);
            DEBUG_TIMER.Elapsed += sendMessage;
            DEBUG_TIMER.Enabled = true;

            graphics = new GraphicsDeviceManager(this); 
            Content.RootDirectory = "Content";
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MonoFont = Content.Load<SpriteFont>("MonoFont");
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
                if (!exiting)
                {
                    _Connection.EndConnection();
                    DEBUG_TIMER.Enabled = false;
                }

                exiting = true;              
                Exit();                
            }

            base.Update(gameTime); 
        }


        private void sendMessage(Object source, System.Timers.ElapsedEventArgs e)
        {
            int num = random.Next(0, 5);
            if (num == 0 || num == 2)
                _Connection.SendMessageRequest(Environment.NewLine + DateTime.Now.ToString("hh:mm:ss: ") + "A fancy new message!"); 

            else
                _Connection.SendMessageRequest(Environment.NewLine + DateTime.Now.ToString("hh:mm:ss: ") + "A new message!");
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.DrawString(MonoFont, _serverMessage, new Vector2(50, 200), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void UpdateFromServer(string message)
        {
            _serverMessage = message;
        }
    }
}
