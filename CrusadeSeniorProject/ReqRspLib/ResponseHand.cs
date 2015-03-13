using System;
using System.Collections.Generic;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseHand : IResponse
    {
        private List<ClientCard> hand;

        public ResponseHand(List<ClientCard> cardList)
        {
            hand = cardList;
        }

        public void Execute(ICrusadeClient client)
        {
            client.SetHand(hand);
        }
    }
}
