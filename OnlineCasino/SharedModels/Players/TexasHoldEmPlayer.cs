using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    [Serializable]
    public class TexasHoldEmPlayer : Player
    {
        public readonly List<Card> Hand;
        public readonly decimal UserBuyIn;
        public decimal UserBet;

        public TexasHoldEmPlayer(User user, int seat, int gameBalance)
            :base(user, seat, gameBalance)
        { }
        
    }
}
