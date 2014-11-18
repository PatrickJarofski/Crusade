using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    /// <summary>
    /// RequestType is a prepended byte that is tacted on
    /// to a client's request that identifies what kind of
    /// request it is.
    /// 
    /// Client_Request: A request that deals with connection requests 
    ///                 or other client-server communication.
    ///                 
    /// Game_Request: A request that deals with game status such as player
    ///               actions or game state.
    ///               
    /// Message_Request: A request to relay a simple message.
    /// </summary>
    public static class RequestResponse
    {
        public enum RequestType : byte { ClientRequest, GameRequest, MessageRequest };

        public enum ResponseType : byte { ClientResponse, GameResponse, MessageResponse };
    }
}
