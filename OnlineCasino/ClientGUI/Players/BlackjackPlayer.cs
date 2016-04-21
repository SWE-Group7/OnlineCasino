using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.Players
{
    public class BlackjackPlayer : CardPlayer
    {        
        public BlackjackPlayer(SharedModels.Players.BlackjackPlayer B):
            base(B, B.UserBet, B.UserBuyIn)
        {
        }
    }
}
