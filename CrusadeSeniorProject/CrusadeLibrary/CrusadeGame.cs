using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class CrusadeGame
    {
        #region Members

         private Player _player1;
         private Player _player2;

         private Player currentPlayer;

         private Gameboard _board;

        #endregion


        #region Methods

        // Eventually take in two clients as parameters??
        public CrusadeGame()
        {
            _board = new Gameboard();

            _player1 = new Player();
            _player2 = new Player();

            currentPlayer = _player1;
        }


        public void BeginNextTurn()
        {
            if (currentPlayer == _player1)
                currentPlayer = _player2;

            else
                currentPlayer = _player1;

            currentPlayer.DrawFromDeck();
        }


        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }

        #endregion

    }
}
