﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class StateTieGame : State
    {
        public static readonly Guid TIE_GAME = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

        private string _name = "StateTieGame";

        public override string Name
        {
            get { return _name; }
        }

        public override State entry(CrusadeGame game, object obj)
        {
            return this;
        }

        public override Guid GetWinner(CrusadeGame game)
        {
            return TIE_GAME;
        }
    }
}
