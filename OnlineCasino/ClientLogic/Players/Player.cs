﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Players;
using SharedModels.GameComponents;
using SMP = SharedModels.Players;
using SharedModels;
using SharedModels.Connection;
using System.Collections.Concurrent;

namespace ClientLogic.Players
{
    public abstract class Player
    {
        public readonly User User;
        public int GameBalance;
        public int Bet;
        public int BuyIn;
        public int Gains;
        public readonly int Seat;
        public WinLossStates WinLossState;
        private bool hasBet;

        public Player(SMP.Player player)
        {
            this.User = new User(player.CurrentUser);
            this.GameBalance = player.GameBalance;
            this.Seat = player.Seat;
            this.Gains = 0;
        }

        public void SetBet(int bet)
        {
            Bet = bet;
            hasBet = true;
        }

        public bool HasBet()
        {
            return hasBet;
        }

        public virtual void RoundReset()
        {
            hasBet = false;
            Bet = 0;
            Gains = 0;
        }

        
    }
}
