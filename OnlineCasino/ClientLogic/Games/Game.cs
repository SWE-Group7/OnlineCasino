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
        public SMG.GameStates GS;

        public int Turn;

        public Game(SMG.Game game)
        {
            Players = new ConcurrentDictionary<int, Player>();
            ServerGameState = game.GameState;

            GS = SMG.GameStates.Waiting;
        }


        public abstract void HandleEvent(GameEvent gameEvent);

        public abstract void Bet(int bet);
        
      
    }

    #region Enums

    #endregion
}


