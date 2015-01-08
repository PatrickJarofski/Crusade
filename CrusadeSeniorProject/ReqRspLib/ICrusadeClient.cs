using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public interface ICrusadeClient
    {
        Guid ID { get; }

        void DisplayHand(List<string> hand);
    }
}
