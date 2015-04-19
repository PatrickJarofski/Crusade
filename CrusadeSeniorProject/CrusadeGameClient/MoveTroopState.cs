using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    internal class MoveTroopState : BoardScreenState
    {
        readonly GameCell cell;
        readonly GameCell[,] board;

        private GameCell targetCell;

        bool selectionDone = false;

        public MoveTroopState(GameCell selectedCell, GameCell[,] gameboard)
            : base()
        {
            cell = selectedCell;
            board = gameboard;
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                return new AwaitUserInputState();

            base.Update(gameTime, previous, current);

            updateCursor();
            if (mouseClicked())
                handleMouseClick();

            if (selectionDone)
                return new AwaitUserInputState();
            else
                return this;
        }

        private void handleMouseClick()
        {
            if(targetCell != null && cursorImage == validChoiceCursor)
            {
                ServerConnection.Instance.MoveTroop(cell, targetCell);
                selectionDone = true;
            }
        }

        private bool mouseClicked()
        {
            return (previousMouseState.LeftButton == ButtonState.Pressed) && (currentMouseState.LeftButton == ButtonState.Released);
        }

        private void updateCursor()
        {
            if (mouseOverValidCell())
                cursorImage = validChoiceCursor;
            else
                cursorImage = invalidChoiceCursor;
        }

        private bool mouseOverValidCell()
        {            
            int xMax = cell.Region.Right + (cell.GamepieceImg.Gamepiece.Move * cell.Image.Width);
            int yMax = cell.Region.Bottom + (cell.GamepieceImg.Gamepiece.Move * cell.Image.Height);
            int xMin = cell.Region.Left - (cell.GamepieceImg.Gamepiece.Move * cell.Image.Width);
            int yMin = cell.Region.Top - (cell.GamepieceImg.Gamepiece.Move * cell.Image.Height);

            bool left = mouseInRange(xMin, cell.Region.Left - 1, currentMouseState.X) &&
                         mouseInRange(cell.Y, cell.Y + cell.Image.Height, currentMouseState.Y);

            bool right = mouseInRange(cell.Region.Right + 1, xMax, currentMouseState.X) &&
                         mouseInRange(cell.Y, cell.Y + cell.Image.Height, currentMouseState.Y);

            bool up = mouseInRange(cell.X, cell.X + cell.Image.Width, currentMouseState.X) &&
                      mouseInRange(yMin, cell.Region.Top - 1, currentMouseState.Y);

            bool down = mouseInRange(cell.X, cell.X + cell.Image.Width, currentMouseState.X) && 
                        mouseInRange(cell.Region.Bottom + 1, yMax, currentMouseState.Y);

            if ((left || right || up || down) && isEmptyCell())
                return true;

            return false;
        }

        private bool isEmptyCell()
        {
            targetCell = getGameCell();

            if (targetCell != null)
                return targetCell.GamepieceImg == null;

            return false;
        }


        private GameCell getGameCell()
        {
            foreach(GameCell cell in board)
            {
                if (mouseInRange(cell.Region.Left, cell.Region.Right, currentMouseState.X) && 
                    mouseInRange(cell.Region.Top, cell.Region.Bottom, currentMouseState.Y))
                    return cell;
            }
            return null;
        }

    }
}