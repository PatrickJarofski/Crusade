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
        private List<string> hand;

        public ResponseHand(List<string> cardList)
        {
            hand = cardList;
        }

        public void Execute(ICrusadeClient client)
        {
            client.DisplayHand(hand);
        }
    }
}
