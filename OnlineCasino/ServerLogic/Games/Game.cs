using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public abstract class Game
    {
        protected List<Player> Players;

        public Game()
        {
            Players = new List<Player>();
        }
    }
}
