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

        private bool exiting = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont MonoFont;

        private string _serverMessage = "No new message.";
        static Random random = new Random();

        public CrusadeGameClient()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            try
            {
                _Connection = new ServerConnection(this);                
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                ServerConnection.WriteToErrorLog(ex.Message);
            }
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
        protected async override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _Connection.EndConnection();
                Exit();                
            }                      


                Thread.Sleep(50);
                await sendMessage();
                sendDone.WaitOne();
                sendDone.Reset();

            base.Update(gameTime); 
        }


        private async Task sendMessage()
        {
            int num = random.Next(0, 11);
            if (num == 5 || num == 10)
                _Connection.SendMessage(DateTime.Now.ToString("HH:mm:ss: ") + "A fancy new message!"); 

            else
                _Connection.SendMessage(DateTime.Now.ToString("HH:mm:ss: ") + "A new message!");

            await Task.Delay(2000);
            sendDone.Set();
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

            spriteBatch.DrawString(MonoFont, _serverMessage, new Vector2(200, 200), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void UpdateFromServer(string message)
        {
            _serverMessage = message;
        }
    }
}
