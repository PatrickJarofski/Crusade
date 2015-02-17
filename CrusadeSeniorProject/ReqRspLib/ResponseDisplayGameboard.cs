using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseDisplayGameboard : IResponse
    {
        public void Execute(ICrusadeClient client)
        {
            client.DisplayGameboard();
        }
    }
}
