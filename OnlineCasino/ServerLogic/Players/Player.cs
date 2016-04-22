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
        protected decimal GameBalance;
        decimal BuyIn = 0;

        public Player(User user)
        {
            //base class. Abstract
            CurrentUser = user;
            GameBalance = BuyIn;
        }

        public void UpdateUserBalance()
        {
            CurrentUser.Balance = CurrentUser.Balance - BuyIn + GameBalance;
        }

        public decimal getGameBalance()
        {
            return GameBalance;
        }

        public string GetFullName()
        {
            return CurrentUser.FullName;
        }

        public string GetUsername()
        {
            return CurrentUser.Username;
        }

        public decimal GetBalance()
        {
            return CurrentUser.Balance;
        }

        public void GiftMoney(decimal gift)
        {
            CurrentUser.Balance += gift;
        }

        private abstract class ClientHelper { }
    }
}
