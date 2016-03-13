using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Games
{
    class Blackjack : Game
    {
        private List<BlackjackPlayer> blackjackPlayers = null;
        public List<BlackjackPlayer> BlackjackPlayers
        {
            get
            {
                if(blackjackPlayers == null)
                    blackjackPlayers = Players.OfType<BlackjackPlayer>().ToList();
                
                return blackjackPlayers;
            }
        }
    }
}
