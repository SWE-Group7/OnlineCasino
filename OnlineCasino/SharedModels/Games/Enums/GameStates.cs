using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Games
{
    public enum GameStates
    {
        Waiting,
        Playing,
        Finializing
    }

    public enum BlackjackStates
    {
        RoundStart,
        Betting,
        Dealing,
        Playing,
        Payout,
        RoundFinish
    }

    public enum RouletteStates
    {
        Betting,
        Spinning,
        GainsOrLoses
    }

    public enum TexasHoldEmStates
    {
        Betting,
        Dealing,
        Playing,
        GainsOrLoses
    }

}
