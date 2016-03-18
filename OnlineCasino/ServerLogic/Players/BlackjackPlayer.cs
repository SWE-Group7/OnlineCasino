using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intermediate.Communications;

namespace ServerLogic.Players
{
    public class BlackjackPlayer : Player
    {
        public BlackjackPlayerStatus Status;
        public float UserBet;
        private List<Card> Cards;

        public BlackjackPlayer(User user, float buyIn)
            : base(user, buyIn)
        {
            Cards = new List<Card>();

            //CurrentUser.Client = new BlackjackConn();
        }

        public bool SetUserBet(float amount)
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
            CurrentUser.Client.IndicateBet();
        }

        public void IndicatePlaying()
        {
            Status = BlackjackPlayerStatus.Playing;
            CurrentUser.Client.IndicatePlaying();
        }

        public void IndicateWait()
        {
            Status = BlackjackPlayerStatus.Waiting;
            CurrentUser.Client.IndicateWaiting();
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

        public void DealCard(Card card) {
            Cards.Add(card);
        }

        public void ForceNoBet()
        {
            Status = BlackjackPlayerStatus.Waiting;
            UserBet = 0;
        }
    }

    public enum BlackjackPlayerStatus
    {
        Waiting,
        Betting,
        Playing
    }
}