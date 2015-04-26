using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class DeployTroopState : BoardScreenState
    {
        CardImage selectedCard;
        GameCell[,] boardCells;

        List<GameCell> cellsHighlighted;
        Texture2D highlight;

        bool deploymentDone = false;

        public DeployTroopState(CardImage img, GameCell[,] board)
            :base()
        {
            cellsHighlighted = new List<GameCell>();
            selectedCard = img;
            boardCells = board;
            highlight = ScreenManager.Instance.Content.Load<Texture2D>("Gameboard/CellMoveRange.png");
            getValidDeployCells();
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            base.Update(gameTime, previous, current);
            updateCursor();

            handleInput();

            if (deploymentDone)
            {
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.NormalCursor;
                return new AwaitUserInputState();
            }
            else
                return this;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (GameCell current in cellsHighlighted)
                spriteBatch.Draw(highlight, new Vector2(current.X, current.Y), Color.White);
        }


        private void handleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                deploymentDone = true;

            else if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                handleMouseClick();
        }


        private void handleMouseClick()
        {
            if (CrusadeGameClient.Instance.Cursor == CrusadeGameClient.Instance.ValidChoiceCursor)
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
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.ValidChoiceCursor;
            }
            else
            {
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.InvalidChoiceCursor;
            }
        }


        private bool mouseOverBackrow()
        {
            if(boardCells != null)
            {
                int width = boardCells[0,0].Image.Width;
                int height = boardCells[0,0].Image.Height;
                int row = ServerConnection.Instance.BackRow;

                for (int col = 0; col < CrusadeGameClient.BOARD_COLS; ++col)
                {
                    int x = boardCells[row, col].X;
                    int y = boardCells[row, col].Y;

                    if (mouseInRange(x, x + width, currentMouseState.X) && mouseInRange(y, y + height, currentMouseState.Y))
                        return true;
                }
            }

            return false;
        }


        private void getValidDeployCells()
        {
            foreach(GameCell current in boardCells)
            {
                if (current.Row == ServerConnection.Instance.BackRow && current.GamepieceImg == null)
                    cellsHighlighted.Add(current);
            }
        }
       
    }
}
