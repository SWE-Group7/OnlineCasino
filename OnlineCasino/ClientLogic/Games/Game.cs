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

        public volatile SMG.GameStates GameState;
        public readonly GameTypes GameType;
        public OverallStates OS = OverallStates.Waiting;
        public WaitingState WS = WaitingState.NoConnection;
        public GameStates GS = GameStates.Waiting;
        public RoundEndStates RES = RoundEndStates.Tie;

        public int Turn;

        public Game(SMG.Game game)
        {
            Players = new ConcurrentDictionary<int, Player>();
            GameState = game.GameState;

            OS = OverallStates.Waiting;
            WS = WaitingState.NoConnection;
            GS = GameStates.Waiting;
            RES = RoundEndStates.Tie;
        }


        public abstract void HandleEvent(GameEvent gameEvent);

        #region Enums
        public enum OverallStates
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
}


