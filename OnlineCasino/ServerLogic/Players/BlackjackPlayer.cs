using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Games;
using SharedModels.GameComponents;


namespace ServerLogic.Players
{
    class BlackjackPlayer : Player
    {
        float UserBet;
        private List<Card> Userhand;
        //cards user holding or things associated with player

        public void set_User_Bet(float amount) { UserBet = amount; }
        
    }
}
