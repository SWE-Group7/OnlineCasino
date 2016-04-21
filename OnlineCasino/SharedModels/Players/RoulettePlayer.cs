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

        public RoulettePlayer(User u, decimal gameBalance, decimal buy, decimal bet) :
            base(u, gameBalance)
        {
            UserBuyIn = buy;
            UserBet = bet;
        }
    }
}
