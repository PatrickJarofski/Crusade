using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class MoveTroopState : BoardScreenState
    {
        private readonly GameCell selectedCell;
        private readonly GameCell[,] board;

        private GameCell targetCell;
        private Texture2D moveHighlight;
        private List<GameCell> cellHighlights;

        private bool selectionDone = false;

        public MoveTroopState(GameCell cell, GameCell[,] gameboard)
            : base()
        {
            selectedCell = cell;
            board = gameboard;
            cellHighlights = new List<GameCell>();
            moveHighlight = ScreenManager.Instance.Content.Load<Texture2D>("Gameboard/CellMoveRange.png");
            getValidMoveCells();
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {            
            base.Update(gameTime, previous, current);

            updateCursor();
            handleInput();

            if (selectionDone)
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
            foreach(GameCell current in cellHighlights)
            {
                spriteBatch.Draw(moveHighlight, new Vector2(current.X, current.Y), Color.White);
            }
        }


        private void handleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                selectionDone = true;

            if (mouseClicked())
                handleMouseClick();
        }


        private void handleMouseClick()
        {
            if(targetCell != null)
            {
                ServerConnection.Instance.MoveTroop(selectedCell, targetCell);
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
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.InvalidChoiceCursor;
            else
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.NormalCursor;
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

        private void getValidMoveCells()
        {
            foreach (GameCell current in board)
                if (cellInMoveRange(current))
                    cellHighlights.Add(current);
        }

        private bool cellInMoveRange(GameCell destinationCell)
        {
            int moveCost = Math.Abs(destinationCell.Row - selectedCell.Row) + Math.Abs(destinationCell.Col - selectedCell.Col);
            return selectedCell.GamepieceImg.Gamepiece.Move >= moveCost && destinationCell != selectedCell && destinationCell.GamepieceImg == null;
        }
    }
}