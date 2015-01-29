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
        ICard _card;

        public ResponsePlayCard(ICard card)
        {
            _card = card;
        }

        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("==== {0} was played. ====", _card.Name);
            Console.WriteLine(Environment.NewLine);
        }
    }
}
