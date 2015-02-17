using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponsePlayCard : IResponse
    {
        private readonly string _card;
        private readonly int _col;
        private readonly int _row;

        public ResponsePlayCard(string card)
        {
            _card = card;
            _col = -1;
            _row = -1;
        }

        public ResponsePlayCard(string card, int row, int col)
        {
            _card = card;
            _col = col;
            _row = row;
        }

        public void Execute(ICrusadeClient client)
        {           
            Console.WriteLine(Environment.NewLine);

            if (_row == -1 && _col == -1)
                Console.WriteLine("==== {0} was played. ====", _card);

            else
                Console.WriteLine("==== {0} was deployed at row {1}, col {2}. ====", _card, _row.ToString(), _col.ToString());

            Console.WriteLine(Environment.NewLine);
        }
    }
}
