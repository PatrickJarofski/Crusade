﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class TroopCard : Card
    {
        #region Members
        private readonly Guid _guid;
        private readonly string _name;

        private CardLocation _cardLocation;
        private int _currentHP;
        private int _attack;
        private int _defense;
        private int _minAttackRange;
        private int _maxAttackRange;
        private int _moveRange;
        #endregion


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

        public int CurrentHP
        {
            get { return _currentHP; }
            set { _currentHP = value; }
        }

        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        public int Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }

        public int MinAttackRange
        {
            get { return _minAttackRange; }
        }

        public int MaxAttackRange
        {
            get { return _maxAttackRange; }
        }

        public int MoveRange
        {
            get { return _moveRange; }
        }
        #endregion    


        #region Methods
        public TroopCard(string name)
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
