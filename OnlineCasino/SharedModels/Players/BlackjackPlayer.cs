using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    public class BlackjackPlayer
    {
        public List<Card> Hand;
        public int UserBuyIn;
        public decimal UserBet;

        public BlackjackPlayer(int buy, decimal bet)
        {
            Hand = new List<Card>();
            UserBuyIn = buy;
            UserBet = bet;
        }

        public void UpdateHand(Card c)
        {
            Hand.Add(c);
        }
    }
}
