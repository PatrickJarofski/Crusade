﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Gameboard : BaseGameObject
    {
        public const int BOARD_COL = 5;
        public const int BOARD_ROW = 5;

        private GamePieceTroop[,] _board;


        /// <summary>
        /// Constructor
        /// </summary>
        public Gameboard()
        {
            _board = new GamePieceTroop[BOARD_ROW, BOARD_COL];
        }


        /// <summary>
        /// Place a game piece on the board
        /// </summary>
        /// <param name="piece">GamePiece to place</param>
        public void PlaceGamePiece(GamePieceTroop piece)
        {
            try
            {
                Tuple<int, int> coordinates = piece.GetCoordinates();
                if (!CellOccupied(coordinates.Item1, coordinates.Item2))
                    _board[coordinates.Item1, coordinates.Item2] = piece;
                else
                    throw new IllegalActionException("Selected cell is already occupied.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
        }


        /// <summary>
        /// Remove a game piece from specified coordinates
        /// </summary>
        /// <param name="row">Row coordinate</param>
        /// <param name="col">Column coordinate</param>
        public void RemoveGamePiece(int row, int col)
        {
            try
            {
                _board[row, col] = null;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
        }


        public bool CellOccupied(int row, int col)
        {
            try
            {
                if (_board[row, col] != null)
                    return true;

                else
                    return false;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
        }


        public GamePieceTroop GetPiece(int row, int col)
        {
            try
            {
                return _board[row, col];
            }
            catch(IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new IndexOutOfRangeException("Dimensions passed are out of the gameboard's bounds.");
            }
        }


        public void MovePiece(Guid ownerId, int startRow, int startCol, int endRow, int endCol)
        {
            try
            {
                if (CellOccupied(endRow, endCol))
                    throw new IllegalActionException("Target destination is occupied.");

                GamePieceTroop piece = _board[startRow, startCol];
                if (piece.OwnerID != ownerId)
                    throw new IllegalActionException("You do not own that piece.");

                _board[endRow, endCol] = piece;
                RemoveGamePiece(startRow, startCol);
            }
            catch(IndexOutOfRangeException)
            {
                throw new IllegalActionException("Invalid coordinates.");
            }
            catch(ArgumentOutOfRangeException)
            {
                throw new IllegalActionException("Invalid coordinates.");
            }
            catch(NullReferenceException)
            {
                throw new IllegalActionException("Empty Cell Selected.");
            }
        }
    }
}
