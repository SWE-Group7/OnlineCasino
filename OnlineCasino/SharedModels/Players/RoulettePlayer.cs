using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    public class RoulettePlayer : Player
    {
        public int UserBuyIn;
        public int UserBet;

        public RoulettePlayer(User u, int seat, int gameBalance, int buy, int bet) :
            base(u, seat, gameBalance)
        {
            UserBuyIn = buy;
            UserBet = bet;
        }
    }
}
