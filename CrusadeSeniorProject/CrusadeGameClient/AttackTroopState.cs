using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class AttackTroopState : BoardScreenState
    {
        private readonly GameCell cell;
        private readonly GameCell[,] board;

        private GameCell targetCell;
        private bool validSelection = false;

        public AttackTroopState(GameCell gameCell, GameCell[,] gameboard)
        {
            cell = gameCell;
            board = gameboard;
        }


        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            base.Update(gameTime, previous, current);
            updateCursor();

            handleInput();

            if (validSelection)
            {
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.NormalCursor;
                return new AwaitUserInputState();
            }
            else
                return this;
        }


        private void updateCursor()
        {
            if (!validTarget())
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.InvalidChoiceCursor;
            else
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.TargetCursor;
        }


        private void handleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                validSelection = true;
                return;
            }

            if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                handleMouseClick();
        }

        private void handleMouseClick()
        {
            GameCell selectedCell = getGameCell();
            if(selectedCell.GamepieceImg != null && selectedCell.GamepieceImg.Gamepiece.Owner != ServerConnection.Instance.ID.ToString())
            {
                ServerConnection.Instance.AttackTroop(cell.Row, cell.Col, selectedCell.Row, selectedCell.Col);
                validSelection = true;
            }
        }


        private bool validTarget()
        {
            targetCell = getGameCell();

            if(targetCell != null)
            {
                if (targetCell.GamepieceImg != null)
                    return targetCell.GamepieceImg.Gamepiece.Owner != ServerConnection.Instance.ID.ToString();
            }

            return false;
        }


        private GameCell getGameCell()
        {
            foreach (GameCell cell in board)
            {
                if (mouseInRange(cell.Region.Left, cell.Region.Right, currentMouseState.X) &&
                    mouseInRange(cell.Region.Top, cell.Region.Bottom, currentMouseState.Y))
                    return cell;
            }
            return null;
        }

    }
}
