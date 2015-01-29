using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseHand : IResponse
    {
        private List<ICard> hand;

        public ResponseHand(List<ICard> cardList)
        {
            hand = cardList;
        }

        public void Execute(ICrusadeClient client)
        {
            client.Hand = hand;
        }
    }
}
