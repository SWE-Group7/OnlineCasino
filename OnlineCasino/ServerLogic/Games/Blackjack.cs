using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Games
{
    public enum BlackJackStates {
        //flags for states
        betting=00, dealing=01,UserPlaying=10,GainsorLoses=11,

    }
    class Blackjack : Game
    {
        //"dealer" take user turns betting or hit
    }
}
