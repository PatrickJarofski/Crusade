﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class EquipCard : Card
    {
        private readonly Guid _guid;
        private readonly string _name;
        private CardLocation _cardLocation;

        #region Properties
        public Guid ID
        {
            get { return _guid; }
        }

        public string Name
        {
            get { return _name; }
        }

        public CardLocation Location
        {
            get { return _cardLocation; }
            set { _cardLocation = value; }
        }
        #endregion

        #region Methods
        public EquipCard(string name)
            : base(name)
        {

        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
