using SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Games
{
    [Serializable]
    public class Game
    {
        public readonly List<Player> Players;
        public readonly GameStates GameState;
        
        public Game(List<Player> players, GameStates gameState)
        {
            Players = players;
            GameState = gameState;
        }
    }
}
