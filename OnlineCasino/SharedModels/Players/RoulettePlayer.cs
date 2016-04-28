using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    public class RoulettePlayer : Player
    {
        public decimal UserBuyIn;
        public decimal UserBet;

        public RoulettePlayer(User u, int seat, decimal gameBalance, decimal buy, decimal bet) :
            base(u, seat, gameBalance)
        {
            UserBuyIn = buy;
            UserBet = bet;
        }
    }
}
