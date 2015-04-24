using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class CrusadeGame
    {
        public static readonly Random RNG = new Random();


        #region Members
        private Player _player1;
        private Player _player2;

        private Gameboard _board;

        #endregion


        #region Properties

        internal Gameboard Board { get { return _board; } }

        internal Player Player1 { get { return _player1; } }
        internal Player Player2 { get { return _player2; } }

        internal Player CurrentPlayer { get; set; }

        internal State CurrentState { get; set; }
        
        public Guid CurrentPlayerId { get { return CurrentPlayer.ID; } }

        public int CurrentPlayerAP { get { return CurrentPlayer.ActionPoints; } }

        #endregion


        #region Public Methods

        public CrusadeGame(Guid player1Id, Guid player2Id)
        {
            _board = new Gameboard();
            _player1 = new Player(player1Id, ConsoleColor.Red);
            _player2 = new Player(player2Id, ConsoleColor.Blue);

            CurrentState = new StateNewGame().entry(this, null);
        }


        public bool MoveTroop(Guid clientId, int startRow, int startCol, int endRow, int endCol)
        {
            if (clientId != CurrentPlayer.ID)
                throw new IllegalActionException("It is not your turn.");

            CurrentState.MoveTroop(this, clientId, startRow, startCol, endRow, endCol);

            if (nextState())
                return true;
            else
                return false;
            
        }


        public GamePieceTroop[,] GetBoardState()
        {         
            GamePieceTroop[,] board = new GamePieceTroop[Gameboard.BOARD_ROW, Gameboard.BOARD_COL];

            for (int row = 0; row < Gameboard.BOARD_ROW; ++row)
            {
                for(int col = 0; col < Gameboard.BOARD_COL; ++col)
                {
                    if (_board.CellOccupied(row, col))
                        board[row, col] = _board.GetPiece(row, col);

                    else
                        board[row, col] = null;
                }
            }

            return board;
        }


        public List<Card> GetPlayerHand(Player.PlayerNumber player)
        {
            if (player == Player.PlayerNumber.NotAPlayer)
                return null;

            if (player == Player.PlayerNumber.PlayerOne)
                return Player1.GetHand();

            else
                return Player2.GetHand();
        }


        /// <summary>
        /// Plays a card if the playerId matches the current turn player.
        /// </summary>
        /// <param name="playerId">Id of the client/player playing the card</param>
        /// <param name="cardSlot">Index of the card in the hand</param>
        /// <returns>Tuple containg the card and a boolean value indicating
        /// whether or not a new turn has started.</returns>
        public Tuple<ICard, bool> PlayCard(Guid playerId, int cardSlot)
        {
            throw new NotImplementedException("Non-troops card are currently not supported.");
            //try
            //{
            //    return CurrentState.PlayCard(this, playerId, cardSlot);
            //}
            //catch(GameStateException ex)
            //{
            //    throw new GameStateException(ex.Message);
            //}
            //catch(CrusadeLibrary.IllegalActionException ex)
            //{
            //    throw new IllegalActionException(ex.Message);
            //}
        }


        /// <summary>
        /// Plays a card if the playerId matches the current turn player.
        /// </summary>
        /// <param name="playerId">Id of the client/player playing the card</param>
        /// <param name="cardSlot">Index of the card in the hand</param>
        /// <param name="row">Row to deploy the Troop Card.</param>
        /// <param name="col">Column to deploy the Troop Card.</param>
        /// <returns>Tuple containg the card and a boolean value indicating
        /// whether or not a new turn has started.</returns>
        public Tuple<ICard, bool> PlayCard(Guid playerId, int cardSlot, int row, int col)
        {
            try
            {
                ICard card = CurrentState.PlayCard(this, playerId, cardSlot, row, col);

                if (nextState())           
                    return new Tuple<ICard, bool>(card, true);                
                else
                    return new Tuple<ICard, bool>(card, false);
            }
            catch (GameStateException ex)
            {
                throw new GameStateException(ex.Message);
            }
        }       


        public Tuple<bool, List<string>, Guid> TroopCombat(Guid turnPlayer, int atkRow, int atkCol, int defRow, int defCol)
        {
            // return state?
            Tuple<State,List<string>> values = CurrentState.TroopCombat(this, turnPlayer, atkRow, atkCol, defRow, defCol);
            CurrentState = values.Item1;
            Guid winner = CurrentState.GetWinner(this);

            return new Tuple<bool, List<string>, Guid>(nextState(), values.Item2, winner);
        }


        public void PassTurn(Guid turnPlayerId)
        {
            if (turnPlayerId != CurrentPlayer.ID)
                throw new IllegalActionException("It is not your turn.");
            else
            {
                CurrentPlayer.ActionPoints = 0;
                CurrentState.PassTurn(this);
            }
        }


        public int GetDeckSize(Guid playerId)
        {
            if (playerId == Player1.ID)
                return Player1.DeckSize;

            if (playerId == Player2.ID)
                return Player2.DeckSize;

            return 0;
        }

        public ConsoleColor GetPlayerColor(Guid playerId)
        {
            if (playerId == Player1.ID)
                return Player1.PlayerColor;

            if (playerId == Player2.ID)
                return Player2.PlayerColor;

            return ConsoleColor.Black;
        }
        #endregion


        #region Private Methods

        /// <summary>
        /// Deducts an actionPoint from the current player. If their AP < 1, the Current State will move to next.
        /// </summary>
        /// <returns>bool based on whether or not state has changed.</returns>
        private bool nextState()
        {
            --CurrentPlayer.ActionPoints;

            if (CurrentPlayer.ActionPoints < 1)
            {
                CurrentState = CurrentState.GetNextState(this);
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
