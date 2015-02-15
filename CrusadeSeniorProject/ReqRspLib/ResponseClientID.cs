using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseClientID : IResponse
    {
        private Guid _id;

        public Guid ID { get { return _id; } }

        public ResponseClientID(Guid id)
        {
            _id = id;
        }

        public void Execute(ICrusadeClient client)
        {
            throw new NotImplementedException("A ResponseClientID should not be executed.");
        }
    }
}
