using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public class GameScreen
    {
        #region Fields
        protected ContentManager content;

        protected List<CardImage> hand;
        protected List<GamepieceImage> board;
        protected MouseState previousMouseState;
        protected MouseState currentMouseState;

        protected CardImage selectedCard;
        protected GamepieceImage selectedPiece;
        #endregion


        #region Properties
        public List<CardImage> Hand { get { return hand; } }
        public List<GamepieceImage> Board { get { return board; } }
        #endregion


        #region Methods
        public virtual void LoadContent()
        {
            hand = new List<CardImage>();
            board = new List<GamepieceImage>();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }


        public virtual void UnloadContent()
        {
            content.Unload();
        }


        public virtual void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }


        public virtual void DrawHand(SpriteBatch spriteBatch)
        {

        }


        public virtual void DrawHand(SpriteBatch spriteBatch, List<ReqRspLib.ClientCard> hand)
        {

        }


        public virtual void DrawGamePieces(SpriteBatch spriteBatch)
        {

        }


        public virtual void DrawGamePieces(SpriteBatch spriteBatch, ReqRspLib.ClientGamePiece[,] board)
        {

        }

        #endregion
    }
}
