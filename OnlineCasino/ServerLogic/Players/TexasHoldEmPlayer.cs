using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Intermediate.Communications;

namespace ServerLogic.Players
{
    public class TexasHoldEmPlayer : Player
    {
        public TexasHoldEmPlayerStatus Status;
        private List<Card> Cards;
        public bool inGame;
        public decimal Bet;

        public TexasHoldEmPlayer(User user, decimal buyIn)
            : base(user, buyIn)
        {
            Cards = new List<Card>();
            inGame = true;
            //CurrentUser.Client = new TexasHoldEmConn();
        }

        public bool SetUserBet(decimal amount)
        {
            if (Status == TexasHoldEmPlayerStatus.Betting)
            {
                Bet = amount;
                Status = TexasHoldEmPlayerStatus.Waiting;
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
            Status = TexasHoldEmPlayerStatus.Betting;
            //CurrentUser.Client.IndicateBet();
        }

        public void IndicatePlaying()
        {
            Status = TexasHoldEmPlayerStatus.Playing;
            //CurrentUser.Client.IndicatePlaying();
        }

        public void IndicateWait()
        {
            Status = TexasHoldEmPlayerStatus.Waiting;
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
            Status = TexasHoldEmPlayerStatus.Waiting;
            Bet = 0;
        }

        public void ClearCards()
        {
            Cards = new List<Card>();
        }
    }

    public enum TexasHoldEmPlayerStatus
    {
        Waiting,
        Betting,
        folding, 
        checking,
        Playing
    }
}