using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class MoveTroopState : BoardScreenState
    {
        private readonly GameCell cell;
        private readonly GameCell[,] board;

        private GameCell targetCell;
        private List<Texture2D> cellHighlights;

        private bool selectionDone = false;

        public MoveTroopState(GameCell selectedCell, GameCell[,] gameboard)
            : base()
        {
            cell = selectedCell;
            board = gameboard;
            cellHighlights = new List<Texture2D>();
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
            if(targetCell != null)
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
            if (!isEmptyCell())
                cursorImage = invalidChoiceCursor;
            else
                cursorImage = normalCursor;
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