using System;
using System.Collections.Generic;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseHand : IResponse
    {
        private List<string> hand;

        public ResponseHand(List<string> cardList)
        {
            hand = cardList;
        }

        public void Execute(ICrusadeClient client)
        {
            client.SetHand(hand);
        }
    }
}
