using ServerLogic.Connections.CommandHandlers;
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
        public bool inGame;
        public decimal Bet;
        private List<Card> Cards;
        private BlackjackCH Commander;

        public BlackjackPlayer(User user, decimal BuyIn)
            : base(user, BuyIn)
        {

            Cards = new List<Card>();
            user.InGame = true;
            inGame = true;
            
        }
        
        public bool SetUserBet(decimal amount)
        {
            if (Status == BlackjackPlayerStatus.Betting)
            {
                Bet = amount;
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
            if (won) GameBalance += Bet;
            else GameBalance -= Bet;
        }

        public void IndicateBet()
        {
            Status = BlackjackPlayerStatus.Betting;
            
        }

        public void IndicatePlaying()
        {
            Status = BlackjackPlayerStatus.Playing;
            //CurrentUser.Client.IndicatePlaying();
        }

        public void IndicateWait()
        {
            Status = BlackjackPlayerStatus.Waiting;
            //CurrentUser.Client.IndicateWaiting();
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
            Bet = 0;
        }

        public void ClearCards()
        {
            Cards = new List<Card>();
        }

        public class Request
        {
            public Request()
            {
                
            }
        }
    }

    public enum BlackjackPlayerStatus
    {
        Waiting,
        Betting,
        Playing
    }

    
}