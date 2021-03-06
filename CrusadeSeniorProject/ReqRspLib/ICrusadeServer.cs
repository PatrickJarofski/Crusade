﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public interface ICrusadeServer
    {
        void GivePlayerHand(Guid clientId);

        void GivePlayerGameboard(Guid clientId);

        void PlayCard(Guid clientId, int cardNum);

        void PlayCard(Guid clientId, int cardNum, int x, int y);

        void MoveTroop(Guid clientId, int startRow, int startCol, int endRow, int endCol);

        void TroopCombat(Guid clientId, int atkRow, int atkCol, int defRow, int defCol);

        void PassTurn(Guid clientId);

        void RestartGame(Guid clientId);
    }
}
