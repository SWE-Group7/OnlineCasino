using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public abstract class Player
    {
        protected User CurrentUser;
        protected float GameBalance;
        float BuyIn;

        public Player(User user, float buyIn)
        {
            //base class. Abstract
            CurrentUser = user;
            GameBalance = buyIn;
            BuyIn = buyIn;
        }

        public void UpdateUserBalance()
        {
            CurrentUser.Balance = CurrentUser.Balance - BuyIn + GameBalance;
        }

        public float getGameBalance()
        {
            return GameBalance;
        }
    }
}
