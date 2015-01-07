using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class ResponseHand : IResponse
    {
        private List<string> hand;

        public ResponseHand(List<string> cardList)
        {
            hand = cardList;
        }

        public void Execute()
        {
            try
            {
                foreach (string card in hand)
                    Console.WriteLine(card + Environment.NewLine);
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Null hand");
            }
        }
    }
}
