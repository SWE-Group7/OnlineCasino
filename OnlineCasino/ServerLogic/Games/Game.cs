using ServerLogic.Games;
using SharedModels.Connection.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public enum GameStates
    {
        Waiting,
        Playing,
        Finializing
    }

    public abstract class Game
    {
        protected List<Player> Players;
        protected GameStates GameState;

        public Game()
        {  
            Players = new List<Player>();
        }

        public static Type GetGameType(GameType gameType)
        {
            switch (gameType)
            {
                case GameType.Blackjack:
                    return typeof(Blackjack);
                case GameType.Roulette:
                    return typeof(Roulette);
                case GameType.TexasHoldEm:
                    return typeof(TexasHoldEm);
                default:
                    return typeof(Game);
            }
        }

        internal void PlayerDisconnect(User user)
        {
            throw new NotImplementedException();
        }
    }
   
}
