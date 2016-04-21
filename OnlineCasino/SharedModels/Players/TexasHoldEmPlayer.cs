using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    public class TexasHoldEmPlayer : Player
    {
        public List<Card> Hand;
        public decimal UserBuyIn;
        public decimal UserBet;

        public TexasHoldEmPlayer(User u, decimal gameBalance, decimal buy, decimal bet) :
            base(u, gameBalance)
        {
            Hand = new List<Card>();
            UserBuyIn = buy;
            UserBet = bet;
        }
    }
}
