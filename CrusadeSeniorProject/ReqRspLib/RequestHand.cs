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
            try
            {
                Console.WriteLine("\nRequesting hand...");
                server.GivePlayerHand(server.GetMatchingClient(clientId));
            }
            catch(NullReferenceException ex)
            {
                server.WriteErrorToConsole("Request Hand Error: " + ex.Message);
                server.WriteErrorToLog("Request Hand Error: " + ex.Message);
            }
        }
    }
}
