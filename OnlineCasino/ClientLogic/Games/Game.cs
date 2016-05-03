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
        public int Turn;


        public Game(SMG.Game game)
        {
            Players = new ConcurrentDictionary<int, Player>();
            GameState = game.GameState;

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


        public abstract void HandleEvent(GameEvent gameEvent);
    }
}


