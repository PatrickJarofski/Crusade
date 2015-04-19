using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrusadeGameClient
{
    internal class AttackTroopState : BoardScreenState
    {
        readonly GameCell cell;
        readonly GameCell[,] board;

        public AttackTroopState(GameCell gameCell, GameCell[,] gameboard)
        {
            cell = gameCell;
            board = gameboard;
        }

        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            return new AwaitUserInputState();
        }
    }
}
