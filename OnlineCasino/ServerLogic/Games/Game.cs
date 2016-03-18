using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public enum GameStates
    {
        Waiting,
        Playing,
        Finializing
    }

    public abstract class Game
    {
        public List<Player> Players;
        public GameStates GameState;

        public Game()
        {  
            Players = new List<Player>();
        }
    }
   
}
