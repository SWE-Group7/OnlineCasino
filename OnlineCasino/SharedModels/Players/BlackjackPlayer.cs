using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    public class BlackjackPlayer : Player
    {
        public List<Card> Hand;
        public int UserBuyIn;
        public int UserBet;

        public BlackjackPlayer(User u, int seat, int gameBalance, int buy, int bet):
            base(u, seat, gameBalance)
        {
            Hand = new List<Card>();
            UserBuyIn = buy;
            UserBet = bet;
        }
    }
}
