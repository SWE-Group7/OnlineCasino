using ServerLogic.Players;
using SharedModels.Connection.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public abstract class Player
    {
        protected Game CurrentGame;
        protected User CurrentUser;
        protected decimal GameBalance;
        decimal BuyIn = 0;

        public Player(User user, decimal buyIn)
        {
            //base class. Abstract
            CurrentUser = user;
            BuyIn = buyIn;
            GameBalance = buyIn;

            user.SetPlayer(this);
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

        public void ClientDisconnect()
        {

        }

        public static Type GetPlayerType(GameType gameType)
        {
            
            switch (gameType)
            {
                case GameType.Blackjack:
                    return typeof(BlackjackPlayer);
                case GameType.Roulette:
                    return typeof(RoulettePlayer);
                case GameType.TexasHoldEm:
                    return typeof(TexasHoldEmPlayer);
                default:
                    return typeof(Game);
            }
        }

        private abstract class ClientHelper { }
    }
}
