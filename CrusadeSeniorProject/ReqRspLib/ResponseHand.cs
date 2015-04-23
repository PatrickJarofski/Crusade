using System;
using System.Collections.Generic;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseHand : IResponse
    {
        private List<ClientCard> hand;
        int deckCount;

        public ResponseHand(List<ClientCard> cardList, int deckSize)
        {
            hand = cardList;
            deckCount = deckSize;
        }

        public void Execute(ICrusadeClient client)
        {
            client.DeckCount = deckCount;
            client.SetHand(hand);
        }
    }
}
