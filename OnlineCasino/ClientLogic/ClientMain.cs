﻿using ClientLogic.Connections;
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
        public static Player MainPlayer;
        public static Connection MainConnection;
        public static Game MainGame;
        public static ConcurrentQueue<GameEvent> EventQueue;
        public static ConcurrentDictionary<ClientCommands, RequestResult> RequestQueue;

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
            EventQueue = new ConcurrentQueue<GameEvent>();
            RequestQueue = new ConcurrentDictionary<ClientCommands, RequestResult>();
            MainGame = null;
            MainPlayer = null;

            lock (Lock)
            {
                SMG.Game smGame;
                RequestResult result;
                int[] data = new int[] { (int)gameType, (int)MainUser.BuyIn };


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
        public static void QueueRequest(ClientCommands cmd, RequestResult request)
        {
            RequestQueue[cmd] = request;
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
            RequestResult result;
            if(RequestQueue.TryGetValue(cmd , out result))
            {
                result.SetValue(true, ret);
            }
        }
    }
}
