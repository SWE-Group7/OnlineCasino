using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Intermediate.Communications;

namespace ServerLogic.Players
{
    public class BlackjackPlayer : Player
    {
        public BlackjackPlayerStatus Status;
        public int UserBuyIn { get; set; }
        public decimal UserBet { get; set; }
        private List<Card> Cards;
        public bool InGame { get; set; }

        public BlackjackPlayer(User user, decimal buyIn)
            : base(user, buyIn)
        {
            Cards = new List<Card>();
            InGame = true;
            //CurrentUser.Client = new BlackjackConn();
        }

        public bool SetUserBet(decimal amount)
        {
            if (Status == BlackjackPlayerStatus.Betting)
            {
                UserBet = amount;
                Status = BlackjackPlayerStatus.Waiting;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateGameBalance(bool won)
        {
            if (won) GameBalance += UserBet;
            else GameBalance -= UserBet;
        }

        public void IndicateBet()
        {
            Status = BlackjackPlayerStatus.Betting;
        }

        public void IndicatePlaying()
        {
            Status = BlackjackPlayerStatus.Playing;
        }

        public void IndicateWait()
        {
            Status = BlackjackPlayerStatus.Waiting;
        }

        public List<Card> GetCards()
        {
            List<Card> CardsCopy = new List<Card>();

            foreach (Card C in Cards)
            {
                CardsCopy.Add(new Card(C.Suit, C.Rank));
            }

            return CardsCopy;
        }

        public void DealCard(Card card)
        {
            Cards.Add(card);
        }

        public void ForceNoBet()
        {
            Console.Out.Write("\nNo bet\n");
            Status = BlackjackPlayerStatus.Waiting;
            UserBet = 0;
        }

        public void ClearCards()
        {
            Cards = new List<Card>();
        }
    }

    public enum BlackjackPlayerStatus
    {
        Waiting,
        Betting,
        Playing
    }
}