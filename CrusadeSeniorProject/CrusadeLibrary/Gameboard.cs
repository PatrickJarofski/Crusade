using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrusadeLibrary
{
    public class Gameboard : BaseGameObject
    {
        public const int BOARD_COL = 5;
        public const int BOARD_ROW = 5;
        public const int PLAYER1_ROW = 0;
        public const int PLAYER2_ROW = 4;

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
        public void DeployGamePiece(GamePieceTroop piece)
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
                if (CellOccupied(row, col))
                    return _board[row, col];
                else
                    return null;
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
                if (piece.Owner != ownerId)
                    throw new IllegalActionException("You do not own that piece.");

                if (!piece.hasMoveRange(startRow, startCol, endRow, endCol))
                    throw new IllegalActionException("Troop does not have enough movement.");

                _board[endRow, endCol] = piece;
                piece.RowCoordinate = endRow;
                piece.ColCoordinate = endCol;
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
