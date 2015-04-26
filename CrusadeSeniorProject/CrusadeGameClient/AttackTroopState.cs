using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class AttackTroopState : BoardScreenState
    {
        private readonly GameCell selectedCell;
        private readonly GameCell[,] board;

        private GameCell targetCell;

        private List<GameCell> cellHighlights;
        private Texture2D attackHighlight;
        private bool validSelection = false;

        public AttackTroopState(GameCell gameCell, GameCell[,] gameboard)
        {
            cellHighlights = new List<GameCell>();
            selectedCell = gameCell;
            board = gameboard;
            getValidAttackCells();
            attackHighlight = ScreenManager.Instance.Content.Load<Texture2D>("Gameboard/CellAttackRange.png");
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (GameCell cell in cellHighlights)
                spriteBatch.Draw(attackHighlight, new Vector2(cell.X, cell.Y), Color.White);
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
            GameCell target = getGameCell();
            if (target.GamepieceImg != null && target.GamepieceImg.Gamepiece.Owner != ServerConnection.Instance.ID.ToString())
            {
                ServerConnection.Instance.AttackTroop(selectedCell.Row, selectedCell.Col, target.Row, target.Col);
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


        private void getValidAttackCells()
        {
            foreach (GameCell current in board)
                if (cellInAttackRange(current))
                    cellHighlights.Add(current);
        }

        private bool cellInAttackRange(GameCell destinationCell)
        {
            if(destinationCell.GamepieceImg == null)
                return false;

            int range = Math.Abs(destinationCell.Row - selectedCell.Row) + Math.Abs(destinationCell.Col - selectedCell.Col);
            return selectedCell.GamepieceImg.Gamepiece.MinAttackRange <= range && selectedCell.GamepieceImg.Gamepiece.MaxAttackRange >= range
                && destinationCell.GamepieceImg.Gamepiece.Owner != selectedCell.GamepieceImg.Gamepiece.Owner;
        }

    }
}
