using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestTroopCombat : IRequest
    {
        Guid attackerId;
        int attackerRow;
        int attackerCol;
        int defenderRow;
        int defenderCol;

        public RequestTroopCombat(Guid client, int atkRow, int atkCol, int defRow, int defCol)
        {
            attackerId = client;
            attackerRow = atkRow;
            attackerCol = atkCol;
            defenderRow = defRow;
            defenderCol = defCol;
        }

        public void Execute(ICrusadeServer server)
        {
            server.TroopCombat(attackerId, attackerRow, attackerCol, defenderRow, defenderCol);
        }
    }
}
