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
        public void Execute(ICrusadeClient client)
        {
            client.GetPlayerAction();
        }
    }
}
