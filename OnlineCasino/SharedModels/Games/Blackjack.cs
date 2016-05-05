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

        public Blackjack(List<Player> players, GameStates gameState, BlackjackStates blackjackState)
            :base(players, gameState)
        {
            BlackjackState = blackjackState;
        }

            
    }
}
