﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseTest : IResponse
    {
        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine("I have no idea what I'm doing.");
        }
    }
}
