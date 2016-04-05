using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Players
{
    public class RoulettePlayer : Player
    {
        public RoulettePlayerStatus Status;
        public int UserBuyIn;
        public decimal UserBet;
        public bool inGame;
        public bool hasBetColor;
        public int userBetNum;
        public string userBetColor;

        public RoulettePlayer(User user, decimal buyIn)
            : base(user, buyIn)
        {
            userBetNum = -1;
            userBetColor = null;
            inGame = true;
        }

        public string getBetColor()
        {
            return userBetColor;
        }

        public int getBetNum()
        {
            return userBetNum;
        }

        public void IndicateBet()
        {
            Status = RoulettePlayerStatus.Betting;
        }

        public void IndicateWait()
        {
            Status = RoulettePlayerStatus.Waiting;
        }

        public void IndicatePlaying()
        {
            Status = RoulettePlayerStatus.Playing;
        }

        public void ForceNoBet()
        {
            Console.Out.Write("\nNo bet\n");
            Status = RoulettePlayerStatus.Waiting;
            UserBet = 0;
        }

        public bool SetUserBet(decimal amount, string color)
        {
            if (Status == RoulettePlayerStatus.Betting)
            {
                UserBet = amount;
                userBetColor = color;
                Status = RoulettePlayerStatus.Waiting;
                hasBetColor = true;
                return true;
            }

            else
                return false;
        }

        public bool SetUserBet(decimal amount, int number)
        {
            if (Status == RoulettePlayerStatus.Betting)
            {
                UserBet = amount;
                userBetNum = number;
                Status = RoulettePlayerStatus.Waiting;
                hasBetColor = false;
                return true;
            }

            else
                return false;
        }

        public void UpdateGameBalance(bool won)
        {
            if (won && hasBetColor) GameBalance += UserBet;
            else if (won && !hasBetColor) GameBalance += (UserBet * 35);
            else GameBalance -= UserBet;
        }

        public void ClearBets()
        {
            userBetNum = -1;
            userBetColor = null;
        }

        public enum RoulettePlayerStatus
        {
            Waiting,
            Betting,
            Playing
        }
    }
}
