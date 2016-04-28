using ClientLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Games
{
    public abstract class Game
    {
        public enum OverallState
        {
            Waiting = 0,
            Playing,
            Distributing
        }
        public OverallState OS = OverallState.Waiting;

        public enum WaitingState
        {
            NoConnection = 0,
            TableFound,

        }
        public WaitingState WS = WaitingState.NoConnection;

        public enum GameState
        {
            Waiting = 0,
            Betting,
            Playing
        }
        public GameState GS = GameState.Waiting;

        public enum RoundEndState
        {
            Win = 0,
            Lose,
            Tie
        }
        public RoundEndState RES = RoundEndState.Tie;

        User CurrentUser;
        List<User> JoinedUsers;

        public Game()
        {
           
        }
    }
}


