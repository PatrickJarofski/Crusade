using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseBadMove : IResponse
    {
        private string _error;

        public ResponseBadMove(string error)
        {
            _error = error;
        }

        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine("\nERROR: {0}", _error);
        }
    }
}
