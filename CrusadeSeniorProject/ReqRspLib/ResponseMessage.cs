using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseMessage : IResponse
    {
        private string _msg;

        public ResponseMessage(string msg)
        {
            _msg = msg;
        }

        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine(_msg);
        }
    }
}
