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
        Color playerColor;

        public ReqRspLib.ClientGamePiece Gamepiece { get { return gamepiece; } }

        public GamepieceImage(string path, int x, int y, ReqRspLib.ClientGamePiece piece)
            :base(path, x, y)
        {
            try
            {
                gamepiece = piece;
                image = ScreenManager.Instance.Content.Load<Texture2D>(path);
                image.GraphicsDevice.Clear(Color.Red);
                int x2 = xLoc * 68 + 155;
                int y2 = yLoc * 68 + 5;
                rec = new Rectangle(x2, y2, image.Width, image.Height);

                playerColor = getColor();
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
                    spriteBatch.Draw(image, rec, playerColor);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("GamepieceImage Error: " + ex.Message);
                }
            }
        }

        private Color getColor()
        {
            if (ServerConnection.Instance.PlayerColor == ConsoleColor.Red)
            {
                if (gamepiece.Owner == ServerConnection.Instance.ID.ToString())
                    return Color.Red;
                else
                    return new Color(50, 80, 255);
            }
            else if(ServerConnection.Instance.PlayerColor == ConsoleColor.Blue)
            {
                if (gamepiece.Owner == ServerConnection.Instance.ID.ToString())
                    return new Color(50, 80, 255);
                else
                    return Color.Red;
            }

            return Color.White;
        }
    }
}
