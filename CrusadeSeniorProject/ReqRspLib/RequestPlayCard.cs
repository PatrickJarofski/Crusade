using System;

namespace ReqRspLib
{
    [Serializable]
    public class RequestPlayCard : IRequest
    {
        private readonly Guid _id;
        private readonly int _cardSlot;
        private readonly int _row;
        private readonly int _col;

        public RequestPlayCard(Guid id, int cardNum)
        {
            _id = id;
            _cardSlot = cardNum;
            _row = -1;
            _col = -1;            
        }

        public RequestPlayCard(Guid id, int cardNum, int row, int col)
        {
            _id = id;
            _cardSlot = cardNum;
            _row = row;
            _col = col;            
        }

        public void Execute(ICrusadeServer server)
        {
            Console.WriteLine("Playing a card...");

            if (_col != -1 && _row != -1)
                server.PlayCard(_id, _cardSlot, _row, _col);
            else
                server.PlayCard(_id, _cardSlot);
        }
    }
}
