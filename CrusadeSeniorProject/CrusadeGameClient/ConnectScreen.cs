using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeGameClient
{
    public class ConnectScreen : GameScreen
    {
        string path;
        Texture2D image;

        public override void LoadContent()
        {
            base.LoadContent();
            path = "Debug/stars.jpg";
            image = content.Load<Texture2D>(path);
        }

        public override void UnloadContent()
        {
            image.Dispose();
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, Vector2.Zero, Microsoft.Xna.Framework.Color.White);
        }
    }
}
