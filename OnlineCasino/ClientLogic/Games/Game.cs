using ClientLogic.Players;
using System;
using System.Collections.Concurrent;
using SMG = SharedModels.Games;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Connection;
using SharedModels;
using SharedModels.Connection.Enums;
using SharedModels.Games.Events;
using SMP = SharedModels.Players;

namespace ClientLogic.Games
{
    public abstract class Game
    {

        public ConcurrentDictionary<int, Player> Players;

        public volatile SMG.GameStates ServerGameState;
        public readonly GameTypes GameType;
        public OverallStates OS;
        public WaitingStates WS;
        public GameStates GS;
        public RoundEndStates RES;

        public int Turn;

        public Game(SMG.Game game)
        {
            Players = new ConcurrentDictionary<int, Player>();
            ServerGameState = game.GameState;

            OS = OverallStates.Waiting;
            WS = WaitingStates.NoConnection;
            GS = GameStates.Waiting;
            RES = RoundEndStates.Tie;
        }


        public abstract void HandleEvent(GameEvent gameEvent);

      
    }

    #region Enums
    public enum OverallStates
    {
        Waiting = 0,
        Playing,
        Distributing
    }


    public enum WaitingStates
    {
        NoConnection = 0,
        TableFound,
    }


    public enum GameStates
    {
        Waiting = 0,
        Betting,
        Playing
    }


    public enum RoundEndStates
    {
        Win = 0,
        Lose,
        Tie
    }

    #endregion
}


