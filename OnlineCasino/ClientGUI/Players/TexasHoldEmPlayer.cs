using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.Players
{
    public class TexasHoldEmPlayer : CardPlayer
    {
        public TexasHoldEmPlayer(SharedModels.Players.TexasHoldEmPlayer T):
            base(T, T.UserBuyIn, T.UserBet)
        {
        }

    }
}
