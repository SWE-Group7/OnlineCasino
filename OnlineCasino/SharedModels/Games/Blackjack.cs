using SharedModels.GameComponents;
using SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Games
{
    [Serializable]
    public class Blackjack : Game
    {
        public readonly BlackjackStates BlackjackState;
        public readonly List<Card> DealerHand;
        public Blackjack(List<Player> players, GameStates gameState, BlackjackStates blackjackState, List<Card> dealerHand = null)
            :base(players, gameState)
        {
            BlackjackState = blackjackState;
            DealerHand = dealerHand;
        }

            
    }
}
