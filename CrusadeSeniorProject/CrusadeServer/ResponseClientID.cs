﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
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

        public void Execute()
        {
            throw new NotImplementedException("A ResponseClientID should not be executed.");
        }
    }
}
