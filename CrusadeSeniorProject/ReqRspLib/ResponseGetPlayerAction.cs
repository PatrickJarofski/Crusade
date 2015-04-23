using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGetPlayerAction : IResponse
    {
        int ap;

        public ResponseGetPlayerAction(int actionPoints)
        {
            ap = actionPoints;
        }

        public void Execute(ICrusadeClient client)
        {
            client.GetPlayerAction();
            client.ActionPoints = ap;
        }
    }
}
