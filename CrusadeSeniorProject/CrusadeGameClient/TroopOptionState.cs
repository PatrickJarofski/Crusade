using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class TroopOptionState : BoardScreenState
    {
        Texture2D menuImage;
        Texture2D highlightMask;
        Rectangle rec;
        GameCell cell;
        const string path = "Gameboard/TroopOptionMenu.png";

        public TroopOptionState(GameCell cell)
            :base()
        {
            this.cell = cell;
            
        }

        public override void LoadContent()
        {
            base.LoadContent();
            menuImage = ScreenManager.Instance.Content.Load<Texture2D>(path);
            rec = new Rectangle(cell.X, cell.Y, menuImage.Width, menuImage.Height);
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                return new AwaitUserInputState();
            
            return base.Update(gameTime, previous, current);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                base.Draw(spriteBatch);
                spriteBatch.Draw(menuImage, rec, Color.White);
            }
            catch(Exception ex)
            {
                ServerConnection.Instance.WriteError("TroopOptionState Error: " + ex.Message);
            }
        }
    }
}
