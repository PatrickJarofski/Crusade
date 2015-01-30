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
        private readonly ICard _card;
        private readonly int _col;
        private readonly int _row;

        public ResponsePlayCard(ICard card)
        {
            _card = card;
            _col = -1;
            _row = -1;
        }

        public ResponsePlayCard(ICard card, int row, int col)
        {
            _card = card;
            _col = col;
            _row = row;
        }

        public void Execute(ICrusadeClient client)
        {           
            Console.WriteLine(Environment.NewLine);

            if (_row == -1 && _col == -1)
                Console.WriteLine("==== {0} was played. ====", _card.Name);

            else
                Console.WriteLine("==== {0} was deployed at row {1}, col {2}. ====", _card.Name, _row.ToString(), _col.ToString());

            Console.WriteLine(Environment.NewLine);
        }
    }
}
