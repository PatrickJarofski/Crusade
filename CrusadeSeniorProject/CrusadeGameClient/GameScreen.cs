using System;
using System.Collections.Generic;
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
        protected MouseState previousMouseState;
        protected MouseState currentMouseState;

        protected CardImage selectedCard;
        protected GamepieceImage selectedPiece;

        protected  bool contentLoaded;
        #endregion


        #region Properties
        public List<CardImage> Hand { get { return hand; } }

        public bool ContentLoaded { get { return contentLoaded; } }

        #endregion


        #region Methods
        public virtual void LoadContent()
        {
            hand = new List<CardImage>();
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

        public virtual void UpdateHand(List<ReqRspLib.ClientCard> newHand)
        {

        }

        public virtual void UpdateBoard(ReqRspLib.ClientGamePiece[,] newBoard)
        {

        }

        #endregion
    }
}
