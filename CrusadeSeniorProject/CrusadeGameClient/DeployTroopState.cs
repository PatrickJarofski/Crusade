using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    internal class DeployTroopState : BoardScreenState
    {
        CardImage selectedCard;
        List<GameCell> boardCells;

        bool deploymentDone = false;

        public DeployTroopState(CardImage img, List<GameCell> board)
            :base()
        {
            selectedCard = img;
            boardCells = board;
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            base.Update(gameTime, previous, current);
            updateCursor();

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                handleMouseClick();

            if (deploymentDone)
                return new AwaitUserInputState();
            else
                return this;
        }

        private void handleMouseClick()
        {
           if(cursorImage == validChoiceCursor)
           {
               Tuple<int, int> coords = getCellNumber();
               if(coords != null)
               {
                   ServerConnection.Instance.PlayCard(selectedCard.Index, coords.Item1, coords.Item2);
                   deploymentDone = true;
               }
           }
        }


        private Tuple<int, int> getCellNumber()
        {
            foreach(GameCell cell in boardCells)
            {
                if(mouseInRange(cell.X, cell.X + cell.Image.Width, currentMouseState.X)
                    && mouseInRange(cell.Y, cell.Y + cell.Image.Height, currentMouseState.Y))
                {
                    return new Tuple<int, int>(cell.Row, cell.Col);
                }
            }

            return null;          
        }


        private void updateCursor()
        {
            if (mouseOverBackrow())
            {
                cursorImage = validChoiceCursor;
            }
            else
            {
                cursorImage = invalidChoiceCursor;
            }
        }


        private bool mouseOverBackrow()
        {
            if(boardCells != null)
            {
                int backrow = ServerConnection.Instance.BackRow;
                int width = boardCells[0].Image.Width;
                int height = boardCells[0].Image.Height;

                for (int i = backrow * 5; i < boardCells.Count; i += 5)
                {
                    int x = boardCells[i].X;
                    int y = boardCells[i].Y;

                    if (mouseInRange(x, x + width, currentMouseState.X) && mouseInRange(y, y + height, currentMouseState.Y))
                        return true;
                }
            }

            return false;
        }

        
        private bool mouseInRange(int min, int max, int mouse)
        {
            return mouse >= min && mouse <= max;
        }
    }
}
