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
        public const int BOARD_COLS = ReqRspLib.Constants.BOARD_COLS;
        public const int BOARD_ROWS = ReqRspLib.Constants.BOARD_ROWS;

        private Texture2D normalCursor;
        private Texture2D validChoiceCursor;
        private Texture2D invalidChoiceCursor;
        private Texture2D targetCursor;

        public Texture2D NormalCursor { get { return normalCursor; } }
        public Texture2D ValidChoiceCursor { get { return validChoiceCursor; } }
        public Texture2D InvalidChoiceCursor { get { return invalidChoiceCursor; } }
        public Texture2D TargetCursor { get { return targetCursor; } }
        public Texture2D Cursor { get; set; }

        public static CrusadeGameClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new CrusadeGameClient();

                return instance;
            }
        }

        private readonly ServerConnection _serverConnection;

        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch spriteBatch;

        private static CrusadeGameClient instance;

        private Vector2 mousePos;

        public bool IsTurnPlayer
        {
            get { return _serverConnection.IsTurnPlayer; }
        }
        
        private CrusadeGameClient()
            : base()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            _serverConnection = ServerConnection.Instance;

            IsMouseVisible = false;
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
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);

            normalCursor = ScreenManager.Instance.Content.Load<Texture2D>("Cursors/NormalCursor.png");
            validChoiceCursor = ScreenManager.Instance.Content.Load<Texture2D>("Cursors/ValidChoiceCursor.png");
            invalidChoiceCursor = ScreenManager.Instance.Content.Load<Texture2D>("Cursors/InvalidChoiceCursor.png");
            targetCursor = ScreenManager.Instance.Content.Load<Texture2D>("Cursors/AttackTroopCursor.png");
            Cursor = NormalCursor;
            
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
            if (_serverConnection.HandUpdated)
                ScreenManager.Instance.UpdateHand(_serverConnection.Hand);

            if (_serverConnection.BoardUpdated)
                ScreenManager.Instance.UpdateBoard(_serverConnection.Gameboard);

            ScreenManager.Instance.Update(gameTime);

            mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
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
            spriteBatch.Draw(Cursor, mousePos, Color.White);
            spriteBatch.End();
        }


        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            lock(_serverConnection)
                _serverConnection.Disconnect();

            UnloadContent();
        }     
    }
}
