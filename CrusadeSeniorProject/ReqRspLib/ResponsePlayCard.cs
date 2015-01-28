using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponsePlayCard : IResponse
    {
        string _card;

        public ResponsePlayCard(string card)
        {
            _card = card;
        }

        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine(Environment.NewLine + "{0} was played.", _card);
        }
    }
}
