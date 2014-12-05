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
        private volatile bool inAGame = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont MonoFont;

        private string _serverMessage = "No new messages";
        private object lockObject = new object();

        public static readonly Random RNG = new Random();
        private static System.Timers.Timer DEBUG_TIMER;

        public CrusadeGameClient()
            : base()
        {
            Console.Title = "Game Console";
            _Connection = new ServerConnection(this);

            if (_Connection.Connected)
                Console.WriteLine("Connected to server.");
            else
                Console.WriteLine("Unable to connected to server.");

            DEBUG_TIMER = new System.Timers.Timer(2000);
            //DEBUG_TIMER.Elapsed += sendMessage;
            //DEBUG_TIMER.Enabled = true;
            //DEBUG_TIMER.Start();
            //sendMessage();

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
                    exiting = true;  
                    DEBUG_TIMER.Enabled = false;
                    _serverMessage = "Shutting down...";
                    _Connection.EndConnection();                    
                }
                            
                Exit();                
            }

            base.Update(gameTime); 
        }


        private void sendMessage(Object source, System.Timers.ElapsedEventArgs e)
        {
            sendMessage();
        }

        private void sendMessage()
        {
            if (!inAGame)
            {
                int num = RNG.Next(0, 5);
                if (num == 0 || num == 2)
                    _Connection.SendMessageRequest(DateTime.Now.ToString("hh:mm:ss: ") + "A fancy new message!");

                else
                    _Connection.SendMessageRequest(DateTime.Now.ToString("hh:mm:ss: ") + "A new message!");
            }
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

           // spriteBatch.DrawString(MonoFont, _serverMessage, new Vector2(100, 100), Color.White);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }


        internal void UpdateFromServer(byte responseType, string message)
        {
            _serverMessage = message;
            
            // Some check to see what kind of message it is
            switch(responseType)
            {
                case CrusadeServer.ResponseTypes.ClientResponse:
                    ProcessClientResponse(message);
                    break;

                case CrusadeServer.ResponseTypes.GameResponse:
                    ProcessGameResponse(message);
                    break;

                case CrusadeServer.ResponseTypes.MessageResponse:
                    ProcessMessageResponse(message);
                    break;

                case CrusadeServer.ResponseTypes.BadResponse:
                    ProcessBadResponse(message);
                    break;

                default:
                    throw new FormatException("An invalid response type was received by the game client.");                    
            }
        }

        private void ProcessBadResponse(string message)
        {
            Console.WriteLine(message);
        }


        private void ProcessClientResponse(string message)
        {
            Console.WriteLine(message);

            if (message == CrusadeServer.Responses.GameStarted)
            {
                lock (lockObject)
                {
                    inAGame = true;
                    Console.WriteLine("Game has begun.");
                    _Connection.SendGameRequest(CrusadeServer.Requests.GetGameboard);
                    _Connection.SendGameRequest(CrusadeServer.Requests.GetPlayerhand);
                }
            }
                
            else if (message == CrusadeServer.Responses.GameOver)
                lock (lockObject)
                { inAGame = false; }

            if (!_Connection.Connected)
                throw new Exception("Client is not connected.");
        }


        private void ProcessMessageResponse(string message)
        {
            Console.WriteLine(message);
        }


        private void ProcessGameResponse(string message)
        {
           // Console.WriteLine(message);

            string[] messageParse = message.Split(CrusadeServer.Constants.GameResponseDelimiters);

            switch(messageParse[0])
            {
                case CrusadeServer.Responses.GiveGameboard:
                    DisplayGameboard(messageParse);
                    break;

                case CrusadeServer.Responses.GiveHand:
                    DisplayHand(messageParse);
                    break;

                case CrusadeServer.Responses.CardPlayed:
                    DisplayPlayedCard(messageParse);
                    break;

                default:
                    throw new FormatException("Response received is not valid.");
            }
        }


        private void DisplayPlayedCard(string[] messageParse)
        {
            Console.WriteLine(messageParse[1] + " was played.");
        }


        private void DisplayHand(string[] messageParse)
        {
            Console.WriteLine("Current hand: " + Environment.NewLine);

            char[] delimiters = { '\n' };
            string[] hand = messageParse[1].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < hand.Length; ++i)
                Console.WriteLine("{0}. {1}", (i + 1).ToString(), hand[i]);

            Console.WriteLine(Environment.NewLine);

            int option = -1;
            bool validChoice = false;
            while(!validChoice)
            {
                Console.Write("Choose a card to play: ");
                option = Convert.ToInt32(Console.ReadKey().KeyChar) - 48;

                if ((option - 1) < hand.Length && (option - 1) > -1)
                    validChoice = true;
                else
                    Console.WriteLine("Invalid Option\n");
            }

            _Connection.SendGameRequest(CrusadeServer.Requests.PlayCard + CrusadeServer.Constants.GameResponseDelimiter + option.ToString());
        }


        private void DisplayGameboard(string[] messageParse)
        {
            char[] delimiters = { '|' };
            string[] board = messageParse[1].Split(delimiters);

            Console.WriteLine(Environment.NewLine + "Gameboard state: " + Environment.NewLine);

            for(int i = 2; i < board.Length; ++i)
            {
                if (board[i] == "=")
                    Console.Write(Environment.NewLine);
                else
                    Console.Write(board[i]);
            }

            Console.WriteLine(Environment.NewLine);
        }


    }
}
