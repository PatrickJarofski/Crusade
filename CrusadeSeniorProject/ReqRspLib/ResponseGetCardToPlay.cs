using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseGetCardToPlay : IResponse
    {
        public void Execute(ICrusadeClient client)
        {
            client.GetCardToPlay();
        }
    }
}
