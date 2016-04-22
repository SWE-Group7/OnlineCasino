using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM = SharedModels.Players;

namespace ClientGUI.Players
{
    public class BlackjackPlayer : CardPlayer
    {        
        public BlackjackPlayer(SM.BlackjackPlayer B):
            base(B, B.UserBet, B.UserBuyIn)
        {
        }
    }
}
