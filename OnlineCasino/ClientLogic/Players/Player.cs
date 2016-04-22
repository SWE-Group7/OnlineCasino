using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Players;
using SharedModels.GameComponents;

namespace ClientLogic.Players
{
    public abstract class Player
    {
        protected readonly User CurrentUser;
        protected readonly decimal GameBalance;

        public decimal UserBuyIn;
        public decimal UserBet;

        public Player(SharedModels.Players.Player P, decimal buyIn, decimal bet)
        {
            this.CurrentUser = P.CurrentUser;
            this.GameBalance = P.GameBalance;

            UserBuyIn = buyIn;
            UserBet = bet;
        }
    }
}
