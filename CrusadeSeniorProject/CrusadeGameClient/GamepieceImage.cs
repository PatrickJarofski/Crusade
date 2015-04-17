using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    public class GamepieceImage : CrusadeImage
    {
        ReqRspLib.ClientGamePiece gamepiece;

        public ReqRspLib.ClientGamePiece Gamepiece { get { return gamepiece; } }

        public GamepieceImage(string path, int x, int y, ReqRspLib.ClientGamePiece piece)
            :base(path, x, y)
        {
            try
            {
                gamepiece = piece;
                image = ScreenManager.Instance.Content.Load<Texture2D>(path);
            }
            catch
            {

            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if(path != null && path != String.Empty)
            {
                try
                {                    
                    int x = xLoc * 68 + 155;
                    int y = yLoc * 68 + 5;
                    rec = new Rectangle(x, y, image.Width, image.Height);
                    spriteBatch.Draw(image, rec, Color.White);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("GamepieceImage Error: " + ex.Message);
                }
            }
        }
    }
}
