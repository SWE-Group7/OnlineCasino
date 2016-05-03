using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Games.Enums
{
    public enum OverallState
    {
        Waiting = 0,
        Playing,
        Distributing
    }
    

    public enum WaitingState
    {
        NoConnection = 0,
        TableFound,

    }
    

    public enum GameState
    {
        Waiting = 0,
        Betting,
        Playing
    }
   

    public enum RoundEndState
    {
        Win = 0,
        Lose,
        Tie
    }
    
}
