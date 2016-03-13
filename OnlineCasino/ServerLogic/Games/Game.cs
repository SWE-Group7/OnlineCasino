using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    //outside class but within namespace define states.
    public enum GameStates
    {

        //created flags for states...based on enum understanding
        waiting = 00,
        playing = 01,
        finializing = 10,
    }

    public abstract class Game
    {
        protected List<Player> Players;

        public Game()
        {  //instantiate when game start requested
            Players = new List<Player>();
        }
    }
   
}
