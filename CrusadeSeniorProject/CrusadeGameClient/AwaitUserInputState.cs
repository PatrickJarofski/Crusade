using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CrusadeGameClient
{
    internal class AwaitUserInputState : BoardScreenState
    {
        public override BoardScreenState Update(GameTime gameTime, MouseState previous, MouseState current)
        {
            if (CrusadeGameClient.Instance.Cursor != CrusadeGameClient.Instance.NormalCursor)
                CrusadeGameClient.Instance.Cursor = CrusadeGameClient.Instance.NormalCursor;
            return base.Update(gameTime, previous, current);
        }
        
    }
}
