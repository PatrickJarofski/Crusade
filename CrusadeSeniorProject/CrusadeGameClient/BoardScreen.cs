using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace CrusadeGameClient
{
    public class BoardScreen : GameScreen
    {
        const int CARD_Y_LOC = 360;
        private const int BOARD_WIDTH = 5;
        private const int BOARD_HEIGHT = 5;

        #region Fields
        private string cellPath;
        private string bgPath;
        private Texture2D cellImage;
        private Texture2D backgroundImage;

        private List<GameCell> boardCells;
        private Rectangle bgRec;

        private BoardScreenState currentState;
        #endregion

        #region Properties

        public List<GameCell> GameboardCells { get { return boardCells; } }

        #endregion


        #region Public Methods

        public override void LoadContent()
        {
            base.LoadContent();
            cellPath = "Gameboard/Cell.png";
            bgPath = "Gameboard/Background.png";

            cellImage = content.Load<Texture2D>(cellPath);
            backgroundImage = content.Load<Texture2D>(bgPath);

            bgRec = new Rectangle(0, 0, ScreenManager.SCREEN_WIDTH, ScreenManager.SCREEN_HEIGHT);

            boardCells = new List<GameCell>();
            setupGameboardCells();
            currentState = new AwaitUserInputState();
            currentState.LoadContent();
        }

        
        public override void UnloadContent()
        {
            cellPath = null;
            bgPath = null;
            cellImage.Dispose();
            backgroundImage.Dispose();
            currentState.UnloadContent();
            base.UnloadContent();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            handleMouseState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
                handleMouseClick();

            currentState = currentState.Update(gameTime, previousMouseState, currentMouseState);

            if (!CrusadeGameClient.Instance.IsTurnPlayer && !(currentState is NotTurnPlayerState))
                currentState = new NotTurnPlayerState();

            else if (CrusadeGameClient.Instance.IsTurnPlayer && currentState is NotTurnPlayerState)
                currentState = new AwaitUserInputState();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundImage, bgRec, Color.White);
            DrawGameboard(spriteBatch);
            DrawGamePieces(spriteBatch);
            DrawHand(spriteBatch);
            currentState.Draw(spriteBatch);
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
            board.Clear();
            foreach (ReqRspLib.ClientGamePiece piece in newBoard)
            {
                if (piece != null)
                {
                    string path = "Gameboard/" + piece.Name + ".png";
                    GamepieceImage img = new GamepieceImage(path, piece.ColCoordinate, piece.RowCoordinate);
                    board.Add(img);
                }
            }
        }

        #endregion


        #region Private Methods

        private void setupGameboardCells()
        {
            for (int col = 0; col < BOARD_WIDTH; ++col)
            {
                for (int row = 0; row < BOARD_HEIGHT; ++row)
                {
                    GameCell img = new GameCell(row, col);
                    boardCells.Add(img);
                }
            }
        }


        private void DrawGameboard(SpriteBatch spriteBatch)
        {
            foreach (GameCell cell in boardCells)
                cell.Draw(spriteBatch);
        }


        private void DrawHand(SpriteBatch spriteBatch)
        {
            foreach (CardImage img in hand)
                img.Draw(spriteBatch);
        }


        private void DrawGamePieces(SpriteBatch spriteBatch)
        {
            foreach (GamepieceImage img in board)
                img.Draw(spriteBatch);
        }


        private void handleMouseState()
        {
            checkMouseOnCard();
        }


        private void handleMouseClick()
        {
            CrusadeImage img = getImage();
            if (img != null)
            {
                if (img is CardImage)
                    processCardInput((CardImage)img);

                else if (img is GamepieceImage)
                    processGamepieceInput((GamepieceImage)img);
            }
            
        }


        private void processGamepieceInput(GamepieceImage gamepieceImage)
        {
            
        }


        private void processCardInput(CardImage image)
        {
            if (CrusadeGameClient.Instance.IsTurnPlayer && !(currentState is DeployTroopState))
            {
                currentState = new DeployTroopState(image, GameboardCells);
                currentState.LoadContent();
            }
        }


        private void checkMouseOnCard()
        {
            foreach (CardImage card in Hand)
            {
                int xMin = card.Region.Left;
                int xMax = card.Region.Right;
                int yMin = card.Region.Top;
                int yMax = card.Region.Bottom;

                if (mouseWithinRange(xMin, xMax, currentMouseState.X) && mouseWithinRange(yMin, yMax, currentMouseState.Y))
                {
                    if(selectedCard != card)
                    {
                        if(selectedCard != null)                        
                            selectedCard.Deselect(); // Deselect previous card

                        selectedCard = card;
                        selectedCard.Select();                        
                    }
                }
            }

            if (hand.Count > 0)
            {
                int xMin = hand[0].Region.Left;
                int xMax = hand[hand.Count - 1].Region.Right;
                int yMax = CARD_Y_LOC + hand[0].Image.Height;

                if(!mouseWithinRange(xMin, xMax, currentMouseState.X) || !mouseWithinRange(CARD_Y_LOC, yMax, currentMouseState.Y))                    
                {
                    if(selectedCard != null)
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


        private CrusadeImage getImage()
        {
            CrusadeImage imageToReturn = null;

            foreach(CardImage img in hand)
                if(mouseWithinRange(img.Region.Left, img.Region.Right, currentMouseState.X) && 
                    mouseWithinRange(img.Region.Top, img.Region.Bottom, currentMouseState.Y))
                {
                    imageToReturn = img;
                    return imageToReturn;
                }

            foreach(GamepieceImage img in board)
                if(mouseWithinRange(img.Region.Left, img.Region.Right, currentMouseState.X) &&
                    mouseWithinRange(img.Region.Top, img.Region.Bottom, currentMouseState.Y))
                    imageToReturn = img;

            return imageToReturn;
        }
        #endregion
    }
}
