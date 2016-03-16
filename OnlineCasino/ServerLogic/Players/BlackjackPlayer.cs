using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intermediate.Communications;

namespace ServerLogic.Players
{
    class BlackjackPlayer : Player
    {
        BlackjackPlayerStatus PlayerStatus;
        float UserBet;
        List<Card> Cards = new List<Card>();
        //cards user holding or things associated with player

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

        public void IndicateBet()
        {
            PlayerStatus = BlackjackPlayerStatus.Betting;
            User.Client.IndicateBet();
        }

        public void IndicatePlaying()
        {
            PlayerStatus = BlackjackPlayerStatus.Playing;
            User.Client.IndicatePlaying();
        }

        public List<Card> GetCards()
        {
            List<Card> CardsCopy = new List<Card>();

            foreach(Card C in Cards)
            {
                CardsCopy.Add(new Card(C.Suit, C.Rank));
            }

            return CardsCopy;
        }
    }

    enum BlackjackPlayerStatus 
    {
        Waiting,
        Betting,
        Playing
    }
}
