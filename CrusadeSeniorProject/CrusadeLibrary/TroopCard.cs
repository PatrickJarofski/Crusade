using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class TroopCard : Card, ICard
    {
        #region Members
        private int _attack;
        private int _defense;
        private int _minAttackRange;
        private int _maxAttackRange;
        private int _moveRange;
        #endregion


        #region Properites
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
            : base(name, CardType.Troop)
        {
            // Debug values
            _attack = 1;
            _defense = 1;
            _maxAttackRange = 1;
            _minAttackRange = 1;
            _moveRange = 3;
        }
        #endregion
    }
}
