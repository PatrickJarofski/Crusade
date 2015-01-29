using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeLibrary
{
    public class EquipCard : Card
    {
        #region Methods
        public EquipCard(string name)
            : base(name, CardType.Equip)
        {
            
        }
        #endregion

    }
}
