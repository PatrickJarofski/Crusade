﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace CrusadeGameClient
{
    public class BoardScreen : GameScreen
    {
        const int CARD_Y_LOC = 360;

        #region Fields
        private Texture2D cellImage;
        private Texture2D backgroundImage;
        private Texture2D playerColor;

        private GameCell[,] board;

        private Rectangle bgRec;
        private SpriteFont messageFont;

        private BoardScreenState currentState;
        private GamepiecePreviewBox menuState;

        private int boardMinX;
        private int boardMinY;
        private int boardMaxX;
        private int boardMaxY;

        private Vector2 apLocation;
        private Vector2 deckLocation;
        private Vector2 playerColorTextLocation;
        private Vector2 playerColorLocation;

        private Texture2D blackColor;
        private Texture2D blueColor;
        private Texture2D redColor;
       
        #endregion

        #region Properties

        public GameCell[,] Gameboard { get { return board; } }

        #endregion


        #region Public Methods

        public override void LoadContent()
        {
            base.LoadContent();

            setupImages();
            setupVectors();
            setupGameboardCells();

            currentState = new AwaitUserInputState();
            currentState.LoadContent();
            contentLoaded = true;
        }

        
        public override void UnloadContent()
        {
            try
            {
                cellImage.Dispose();
                backgroundImage.Dispose();
                currentState.UnloadContent();
                base.UnloadContent();
            }
            catch(Exception ex)
            {
                ServerConnection.Instance.WriteError(ex.Message);
            }
        }


        public override void Update(GameTime gameTime)
        {
            try
            {
                base.Update(gameTime);

                playerColor = getPlayerColor();

                if (!CrusadeGameClient.Instance.IsTurnPlayer && !(currentState is NotTurnPlayerState))
                    currentState = new NotTurnPlayerState();

                else if (CrusadeGameClient.Instance.IsTurnPlayer && currentState is NotTurnPlayerState)
                    currentState = new AwaitUserInputState();

                if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                    handleMouseClick();

                currentState = currentState.Update(gameTime, previousMouseState, currentMouseState);
                handleMouseState();

                if (menuState != null)
                    menuState = menuState.Update(gameTime, previousMouseState, currentMouseState);
            }
            catch (Exception ex)
            {
                ServerConnection.Instance.WriteError(ex.Message);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(backgroundImage, bgRec, Color.White);

                DrawGameboard(spriteBatch);
                DrawHand(spriteBatch);
                drawAPandDeckSize(spriteBatch);
                drawPlayerColor(spriteBatch);
                currentState.Draw(spriteBatch);
                if (menuState != null)
                    menuState.Draw(spriteBatch);
            }
            catch(NullReferenceException)
            {
                LoadContent();
            }
            catch(ArgumentNullException)
            {
                LoadContent();
            }
        }
        

        public override void UpdateHand(List<ReqRspLib.ClientCard> newHand)
        {
            hand.Clear();
            for (int i = 0; i < newHand.Count; ++i)
            {
                CardImage newImg = new CardImage("Cards/" + newHand[i].Name + ".png", i, CARD_Y_LOC, newHand[i].Index);
                hand.Add(newImg);
            }
        }


        public override void UpdateBoard(ReqRspLib.ClientGamePiece[,] newBoard)
        {
            for (int row = 0; row < CrusadeGameClient.BOARD_ROWS; ++row)
            {
                for(int col = 0; col < CrusadeGameClient.BOARD_COLS; ++col)
                {
                    if (newBoard[row, col] != null)
                    {
                        string path = "Gameboard/" + newBoard[row, col].Name + ".png";
                        GamepieceImage img = new GamepieceImage(path, newBoard[row, col].ColCoordinate, newBoard[row, col].RowCoordinate, newBoard[row,col]);
                        board[row, col].GamepieceImg = img;
                    }
                    else
                        board[row,col].GamepieceImg = null;
                }
            }
        }

        #endregion


        #region Private Methods
        private void setupImages()
        {
            cellImage = content.Load<Texture2D>("Gameboard/Cell.png");
            backgroundImage = content.Load<Texture2D>("Gameboard/Background.png");
            
            messageFont = content.Load<SpriteFont>("MessageFont");

            blackColor = content.Load<Texture2D>("Gameboard/Black.png");
            blueColor = content.Load<Texture2D>("Gameboard/Blue.png");
            redColor = content.Load<Texture2D>("Gameboard/Red.png");

            bgRec = new Rectangle(0, 0, ScreenManager.SCREEN_WIDTH, ScreenManager.SCREEN_HEIGHT);
        }


        private void setupGameboardCells()
        {
            board = new GameCell[CrusadeGameClient.BOARD_ROWS, CrusadeGameClient.BOARD_COLS];

            for (int row = 0; row < CrusadeGameClient.BOARD_ROWS; ++row)
            {
                for (int col = 0; col < CrusadeGameClient.BOARD_COLS; ++col)
                {
                    GameCell img = new GameCell(row, col);
                    board[row, col] = img;
                }
            }

            boardMinX = board[0, 0].X;
            boardMinY = board[0, 0].Y;
            boardMaxX = board[CrusadeGameClient.BOARD_ROWS - 1, CrusadeGameClient.BOARD_COLS - 1].X
                + board[0, 0].Image.Width; // All cells have the same cell image
            boardMaxY = board[CrusadeGameClient.BOARD_ROWS - 1, CrusadeGameClient.BOARD_COLS - 1].Y
                + board[0, 0].Image.Height;
        }


        private void setupVectors()
        {
            apLocation = new Vector2(5, 25);
            deckLocation = new Vector2(550, 320);
            playerColorTextLocation = new Vector2(5, 100);
            playerColorLocation = new Vector2(85, 100);
        }


        private void DrawGameboard(SpriteBatch spriteBatch)
        {
            foreach (GameCell cell in board)
                cell.Draw(spriteBatch);
        }


        private void DrawHand(SpriteBatch spriteBatch)
        {
            foreach (CardImage img in hand)
                img.Draw(spriteBatch);
        }


        private void drawAPandDeckSize(SpriteBatch spriteBatch)
        {
            string msg;
            if (ServerConnection.Instance.IsTurnPlayer)
                msg = "Action Points: " + ServerConnection.Instance.ActionPoints;
            else
                msg = "Opponent's Turn";

            spriteBatch.DrawString(messageFont, msg, apLocation, Color.White);
            spriteBatch.DrawString(messageFont, "Deck: " + ServerConnection.Instance.DeckCount.ToString(), deckLocation, Color.White);
        }


        private void drawPlayerColor(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(messageFont, "Your Color:", playerColorTextLocation, Color.White);
            spriteBatch.Draw(playerColor, playerColorLocation, Color.White);
        }


        private void handleMouseState()
        {
            doCardHighlighting();
            doCellHighlighting();
        }
        

        private void handleMouseClick()
        {
            if(mouseOverHand())
                processCardInput(getCardImage());

            else if (mouseOverBoard())
                processGamepieceInput(getCell());
        }


        private void processGamepieceInput(GameCell cell)
        {
            if (cell == null || cell.GamepieceImg == null || (cell.GamepieceImg.Gamepiece.Owner != ServerConnection.Instance.ID.ToString()))
                return;

            if(currentState is AwaitUserInputState)
            {
                currentState = new TroopOptionState(cell, Gameboard);
                currentState.LoadContent();
            }
        }


        private void processCardInput(CardImage image)
        {
            if (image == null) 
                return;

            if (CrusadeGameClient.Instance.IsTurnPlayer && currentState is AwaitUserInputState)
            {
                currentState = new DeployTroopState(image, Gameboard);
                currentState.LoadContent();
            }
        }


        private void doCardHighlighting()
        {
            if (!(currentState is AwaitUserInputState))
                return;

            // Only bother checking hand if mouse is within range
            // Mouse X can be anything; only need to check Y coordinate
            if (mouseOverHand())                 
            {
                foreach (CardImage card in Hand)
                {
                    int xMin = card.Region.Left;
                    int xMax = card.Region.Right;
                    int yMin = card.Region.Top;
                    int yMax = card.Region.Bottom;

                    if (mouseWithinRange(xMin, xMax, currentMouseState.X) && mouseWithinRange(yMin, yMax, currentMouseState.Y))
                    {
                        if (selectedCard != card)
                        {
                            if (selectedCard != null)
                                selectedCard.Deselect(); // Deselect previous card

                            selectedCard = card;
                            selectedCard.Select();
                        }
                    }
                }
            }
            checkCursorNotOverHand();
        }


        private void doCellHighlighting()
        {
            // Only bother checking cells if mouse within board bounds
            if (mouseOverBoard()) 
            {
                GameCell cell = getCell();
                if(cell != null && cell.GamepieceImg != null && validCreateMenuState())
                {
                    menuState = new GamepiecePreviewBox(cell);
                    menuState.LoadContent();
                }
            }
        }


        private bool validCreateMenuState()
        {
            if (currentState is AwaitUserInputState)
                return (menuState == null);

            if (currentState is NotTurnPlayerState)
                return (menuState == null);

            if (currentState is AttackTroopState)
                return (menuState == null);

            return false;
        }


        private GameCell getCell()
        {
            for(int row = 0; row < CrusadeGameClient.BOARD_ROWS; ++row)
            {
                for(int col = 0; col < CrusadeGameClient.BOARD_COLS; ++col)
                {
                   if(mouseWithinRange(board[row, col].X, board[row, col].X + board[row, col].Image.Width, currentMouseState.X)
                       && mouseWithinRange(board[row, col].Y, board[row, col].Y + board[row, col].Image.Height, currentMouseState.Y))
                   {
                       return board[row, col];
                   }
                }
            }
            return null;
        }


        private CardImage getCardImage()
        {
            CardImage imageToReturn = null;

            foreach (CardImage img in hand)
                if (mouseWithinRange(img.Region.Left, img.Region.Right, currentMouseState.X) &&
                    mouseWithinRange(img.Region.Top, img.Region.Bottom, currentMouseState.Y))
                imageToReturn = img;          

            return imageToReturn;
        }


        private void checkCursorNotOverHand()
        {
            if (hand.Count > 0)
            {
                int xMin = hand[0].Region.Left;
                int xMax = hand[hand.Count - 1].Region.Right;
                int yMax = CARD_Y_LOC + hand[0].Image.Height;

                if (!mouseWithinRange(xMin, xMax, currentMouseState.X) || !mouseWithinRange(CARD_Y_LOC, yMax, currentMouseState.Y))
                {
                    if (selectedCard != null)
                    {
                        selectedCard.Deselect();
                        selectedCard = null;
                    }
                }
            }
        }


        private bool mouseWithinRange(int min, int max, int mouseCoord)
        {
            return mouseCoord >= min && mouseCoord <= max;
        }

        
        private bool mouseOverHand()
        {
            return mouseWithinRange(boardMaxY + 1, ScreenManager.SCREEN_HEIGHT, currentMouseState.Y);
        }


        private bool mouseOverBoard()
        {
            return (mouseWithinRange(boardMinX, boardMaxX, currentMouseState.X)
                && mouseWithinRange(boardMinY, boardMaxY, currentMouseState.Y));
        }


        private Texture2D getPlayerColor()
        {
            if (ServerConnection.Instance.PlayerColor == ConsoleColor.Red)
                return redColor;
            else if (ServerConnection.Instance.PlayerColor == ConsoleColor.Blue)
                return blueColor;

            return blackColor; // default
        }
        #endregion
    }
}
