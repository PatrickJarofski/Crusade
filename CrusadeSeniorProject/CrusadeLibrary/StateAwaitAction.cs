﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateAwaitAction : State
    {
        private string _name;
        public override string Name { get { return _name; } }

        public StateAwaitAction()
        {
            _name = "Await Action";
        }


        public override State GetNextState(CrusadeGame game)
        {
            return new StateNextPlayerTurn().entry(game, null);
        }


        public override State entry(CrusadeGame game, object obj)
        {
            // stub
            return this;
        }


        public override ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot)
        {
            if (game.CurrentPlayer.ID != playerId)
                throw new IllegalActionException("It is not your turn.");
            else
                throw new NotImplementedException("Non-Troop cards are currently not supported.");
        }


        public override ICard PlayCard(CrusadeGame game, Guid playerId, int cardSlot, int row, int col)
        {
            if (game.CurrentPlayer.ID != playerId)
                throw new IllegalActionException("It is not your turn.");
            else
            {
                try
                {
                    List<Card> hand = game.CurrentPlayer.GetHand();
                    if (hand[cardSlot].Type != CardType.Troop)
                        throw new NotImplementedException("Non-Troop cards are currently not supported.");

                    game.Board.DeployGamePiece(new GamePieceTroop(row, col, playerId, hand[cardSlot].Name));

                    return game.CurrentPlayer.PlayCard(cardSlot);
                }
                catch(IndexOutOfRangeException)
                {
                    throw new IllegalActionException("Game does not recognize card chosen (Out of range).");
                }
                catch(ArgumentOutOfRangeException)
                {
                    throw new IllegalActionException("Game does not recognize card chosen (Out of range).");
                }
            }
        }
        

        public override void MoveTroop(CrusadeGame game, Guid ownerId, int startRow, int startCol, int endRow, int endCol)
        {
            game.Board.MovePiece(ownerId, startRow, startCol, endRow, endCol);
        }


        public override Tuple<State, List<string>> TroopCombat(CrusadeGame game, Guid turnPlayer, int atkRow, int atkCol, int defRow, int defCol)
        {
            if (turnPlayer != game.CurrentPlayer.ID)
                throw new IllegalActionException("It is not your turn.");

            GamePieceTroop atkPiece = game.Board.GetPiece(atkRow, atkCol);
            GamePieceTroop defPiece = game.Board.GetPiece(defRow, defCol);

            if (atkPiece == null || defPiece == null)
                throw new IllegalActionException("Empty cell selected.");

            if (!opposingPieces(atkPiece, defPiece))
                throw new IllegalActionException("You cannot attack troops that you own.");

            if (!inRange(atkPiece, defPiece))
                throw new IllegalActionException("Target is not within range.");

            doCombat(atkPiece, defPiece);
            List<string> troopsDefeated = checkForDefeatedTroops(game, atkPiece, defPiece);
            State nextState = checkState(atkPiece, defPiece);

            return new Tuple<State, List<string>>(nextState, troopsDefeated);
        }


        private bool opposingPieces(GamePieceTroop atkPiece, GamePieceTroop defPiece)
        {
            return atkPiece.Owner != defPiece.Owner;
        }


        private bool inRange(GamePieceTroop atkPiece, GamePieceTroop defPiece)
        {
            return atkPiece.hasAttackRange(atkPiece.RowCoordinate, atkPiece.ColCoordinate, defPiece.RowCoordinate, defPiece.ColCoordinate);
        }


        private void doCombat(GamePieceTroop atkPiece, GamePieceTroop defPiece)
        {
            // We've already confirmed that the attacking piece has enough range for the attack
            atkPiece.RemainingDefense = atkPiece.RemainingDefense - defPiece.Attack;

            // If the defending has the attack range for a counter attack, it should do so
            if(defPiece.hasAttackRange(defPiece.RowCoordinate, defPiece.ColCoordinate, atkPiece.RowCoordinate, atkPiece.ColCoordinate))
                defPiece.RemainingDefense = defPiece.RemainingDefense - atkPiece.Attack;
        }   


        private List<string> checkForDefeatedTroops(CrusadeGame game, GamePieceTroop atkPiece, GamePieceTroop defPiece)
        {
            List<string> troops = new List<string>();
            string defeatMsg = " has been defeated!";

            if (atkPiece.RemainingDefense <= 0)
            {
                game.Board.RemoveGamePiece(atkPiece.RowCoordinate, atkPiece.ColCoordinate);
                troops.Add(atkPiece.Name + defeatMsg);
            }

            if (defPiece.RemainingDefense <= 0)
            {
                game.Board.RemoveGamePiece(defPiece.RowCoordinate, defPiece.ColCoordinate);
                troops.Add(defPiece.Name + defeatMsg);
            }

            return troops;
        }


        private bool gameIsTie(GamePieceTroop atkPiece, GamePieceTroop defPiece)
        {
            return atkPiece.Name == GamePiece.COMMANDER && atkPiece.RemainingDefense <= 0 
                && defPiece.Name == GamePiece.COMMANDER && defPiece.RemainingDefense <= 0;
        }

        private bool defeatedCommander(GamePieceTroop piece)
        {
            return piece.RemainingDefense <= 0;
        }

        private State checkState(GamePieceTroop atkPiece, GamePieceTroop defPiece)
        {
            if (gameIsTie(atkPiece, defPiece))
                return new StateTieGame();            

            if (defeatedCommander(defPiece))
                return new StateAttackerWins();            

            if (defeatedCommander(atkPiece))
                return new StateDefenderWins();            

            // state hasn't changed
            return this;
        }
    }
}
