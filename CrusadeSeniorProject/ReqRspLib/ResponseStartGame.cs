﻿using System;

namespace ReqRspLib
{
    [Serializable]
    public class ResponseStartGame : IResponse
    {
        public void Execute(ICrusadeClient client)
        {
            Console.WriteLine("Start game.");
            client.BeginGame();
        }
    }
}
