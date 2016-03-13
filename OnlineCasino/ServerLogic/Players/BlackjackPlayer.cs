using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Players
{
    class BlackjackPlayer : Player
    {
        float UserBet;
        //cards user holding or things associated with player

        public void set_User_Bet(float amount) { UserBet = amount; }
        
    }
}
