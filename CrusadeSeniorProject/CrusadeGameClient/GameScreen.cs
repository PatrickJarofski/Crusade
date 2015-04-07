using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CrusadeGameClient
{
    public class GameScreen
    {
        protected ContentManager content;

        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }


        public virtual void UnloadContent()
        {
            content.Unload();
        }


        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void DrawHand(SpriteBatch spriteBatch, List<ReqRspLib.ClientCard> hand)
        {

        }

        public virtual void DrawGamePieces(SpriteBatch spriteBatch, ReqRspLib.ClientGamePiece[,] board)
        {

        }
    }
}
