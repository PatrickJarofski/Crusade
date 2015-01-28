using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class RequestPlayCard : IRequest
    {
        Guid _id;
        int cardToPlay;

        public RequestPlayCard(Guid id, int cardNum)
        {
            _id = id;
            cardToPlay = cardNum;
        }

        public void Execute(ICrusadeServer server)
        {
            Console.WriteLine("Playing a card...");
            server.PlayCard(_id, cardToPlay);
        }
    }
}
