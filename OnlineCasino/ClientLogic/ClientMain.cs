using ClientLogic.Connections;
using SharedModels;
using SharedModels.Connection;
using SMP = SharedModels.Players;
using SMG = SharedModels.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientLogic.Players;
using ClientLogic.Games;
using SharedModels.Connection.Enums;
using SharedModels.Games.Events;
using System.Collections.Concurrent;

namespace ClientLogic
{
    public static class ClientMain
    {
        
        public static User MainUser;
        private static Player _MainPlayer;
        public static Player MainPlayer
        {
            get
            {
                return _MainPlayer;
            }

            set
            {
                _MainPlayer = value;
                _MainPlayer.BuyIn = BuyIn;
                _MainPlayer.Bet = Bet;
            }
        }
        public static Connection MainConnection;
        public static Game MainGame;

        public static int BuyIn;
        public static int Bet;

        public static GameTypes GameType = GameTypes.None;
        public static ClientStates ClientState = ClientStates.Login;

        public static ConcurrentQueue<GameEvent> EventQueue = new ConcurrentQueue<GameEvent>();
        public static ConcurrentDictionary<ClientCommands, int> ServerRequests;

        private static object Lock = new object();

        private static string LoginMessage;
        
        public static bool TrySyncLogin(string username, string password)
        {
            MainConnection = new Connection();
            Security security = new Security(username, password);
            return MainConnection.TrySyncLogin(security, out MainUser);
        }
        public static bool TrySyncRegister(string username, string password, string fullName, string email)
        {
            MainConnection = new Connection();
            Security security = new Security(username, password, fullName, email);
            return MainConnection.TrySyncRegister(security, out MainUser);
        }

        public static bool TryJoinGame(GameTypes gameType)
         {
            ServerRequests = new ConcurrentDictionary<ClientCommands, int>();
            foreach(ClientCommands command in Enum.GetValues(typeof(ClientCommands)).Cast<ClientCommands>())
            {
                ServerRequests.TryAdd(command, -1);
            }
            MainGame = null;

            lock (Lock)
            {
                SMG.Game smGame;
                RequestResult result;
                int[] data = new int[] { (int)gameType, (int)BuyIn };


                result = MainConnection.Request(ServerCommands.JoinGame, data);
                if (result.WaitForReturn<SMG.Game>(5000, out smGame))
                {
                    switch (gameType)
                    {
                        case GameTypes.Blackjack:
                            MainGame = new Blackjack((SMG.Blackjack)smGame);
                            break;
                        default:
                            break;
                    }

                }

                return (MainGame != null);
            }
            
        }

        public static void SetLoginMessage(string message)
        {
            LoginMessage = message;
        }
        public static string GetLoginMessage()
        {
            return LoginMessage;
        }

        public static void QueueEvent(GameEvent gameEvent)
        {
            EventQueue.Enqueue(gameEvent);
        }
        public static void QueueRequest(ClientCommands cmd, int reqId)
        {
            ServerRequests[cmd] = reqId;
        }
        public static void HandleEvents()
        {
            GameEvent gameEvent;
            while (EventQueue.TryDequeue(out gameEvent))
            {
                MainGame.HandleEvent(gameEvent);
            }
        }
        public static void HandleRequests(ClientCommands cmd, object ret)
        {
            int reqId;
            if(ServerRequests.TryGetValue(cmd , out reqId))
            {
                MainConnection.Return(reqId, true, ret);
            }
        }
    }

    public enum ClientStates
    {
        Login = 0,
        Register,
        Menu,
        Betting,
        Game
    }
}
