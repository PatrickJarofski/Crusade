using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponsePlayerColor : IResponse
    {
        ConsoleColor color;

        public ResponsePlayerColor(ConsoleColor playerColor)
        {
            color = playerColor;
        }

        public void Execute(ICrusadeClient client)
        {
            client.PlayerColor = color;
        }
    }
}
