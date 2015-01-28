using System;

namespace ReqRspLib
{
    [Serializable]
    public class RequestHand : IRequest
    {
        private Guid clientId;
        
        public RequestHand(Guid id)
        {
            clientId = id;
        }

        public void Execute(ICrusadeServer server)
        {
            server.GivePlayerHand(clientId);
        }
    }
}
