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
        BlackjackPlayerStatus PlayerStatus;
        float UserBet;
        List<Card> Cards;

        public BlackjackPlayer(User user, float buyIn)
            : base(user, buyIn)
        {
            Cards = new List<Card>();

            //CurrentUser.Client = new BlackjackConn();
        }

        public bool SetUserBet(float amount)
        {
            if (PlayerStatus == BlackjackPlayerStatus.Betting)
            {
                UserBet = amount;
                PlayerStatus = BlackjackPlayerStatus.Waiting;
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
            PlayerStatus = BlackjackPlayerStatus.Betting;
            CurrentUser.Client.IndicateBet();
        }

        public void IndicatePlaying()
        {
            PlayerStatus = BlackjackPlayerStatus.Playing;
            CurrentUser.Client.IndicatePlaying();
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

        public void DealtCards(Card card) {
            Cards.Add(card);
        }

        public void ForceNoBet()
        {
            PlayerStatus = BlackjackPlayerStatus.Waiting;
            UserBet = 0;
        }
    }

    enum BlackjackPlayerStatus
    {
        Waiting,
        Betting,
        Playing
    }
}