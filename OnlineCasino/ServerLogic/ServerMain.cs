using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedModels.Connection;
using System.Runtime.Serialization.Formatters.Binary;
using SharedModels;
using ServerLogic.Connections;
using System.Diagnostics;
using SharedModels.Connection.Enums;
using ServerLogic.Games;
using System.Collections.Concurrent;
using SMP = SharedModels.Players;
using SharedModels.Games.Events;
using SharedModels.Games.Enums;

namespace ServerLogic
{
    public static class ServerMain
    {
        

        private static ConcurrentDictionary<int, User> ConnectedUsers;
        private static ConcurrentDictionary<GameTypes, List<Game>> AllGames;
        private static ConcurrentDictionary<ServerCommands, string> ClientErrorMessages;
        private static TcpListener Listener;
        private static Stopwatch Timer = new Stopwatch();
        private static volatile int NextGameID = 1;

        public static bool GameEvents { get; private set; }

        public static void Start()
        {
            ConnectedUsers = new ConcurrentDictionary<int, User>();
            AllGames = new ConcurrentDictionary<GameTypes, List<Game>>();
            ClientErrorMessages = new ConcurrentDictionary<ServerCommands, string>();

            //Initialize list of game for each GameType 
            GameTypes[] gameTypes = (GameTypes[]) Enum.GetValues(typeof(GameTypes));
            gameTypes = gameTypes.Where(i => (int)i != 0).ToArray();
            foreach (GameTypes game in gameTypes)
                AllGames.TryAdd(game, new List<Game>());
            
            Listener = new TcpListener(IPAddress.Any, 47689);
            ListenForClients();
        }

        private static void ListenForClients()
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            byte[] obj;
            int bytesRead;
            int bytesToRead;
            int index;

            Listener.Start();

            CommTypes commType;
            ServerCommands cmd;
            int reqId;
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();

                //Ensure not stuck
                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                //Ensure Command Start
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommTypes)BitConverter.ToInt32(buffer, 0);
                if (commType != CommTypes.Start)
                    continue;

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;
                    
                //Get CommType
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommTypes) BitConverter.ToInt32(buffer, 0);

                //If not Request, Close
                if(commType != CommTypes.Request)
                {
                    client.GetStream().Dispose();
                    client.Close();
                    continue;
                }

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                //Get Command
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ServerCommands) BitConverter.ToInt32(buffer, 0);

                //If not Login or Register, Close
                if (!(cmd == ServerCommands.Login || cmd == ServerCommands.Register))
                {
                    client.GetStream().Dispose();
                    client.Close();
                    continue;
                }

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                //Get Request ID and Object Length
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                reqId = BitConverter.ToInt32(buffer, 0);

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                //Get Object
                obj = new byte[bytesToRead];
                index = 0;
                bool stuck = false;
                while (bytesToRead > 0)
                {
                    if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    {
                        stuck = true;
                        break;
                    }

                    bytesRead = client.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
                    Array.ConstrainedCopy(buffer, 0, obj, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }

                if (stuck)
                    continue;

                Thread thread = new Thread(() => HandleClientLogin(client, cmd, reqId, obj));
                thread.Start();

            }
        }
        private static void HandleClientLogin(TcpClient client, ServerCommands cmd, int reqId, byte[] obj)
        {
            Connection connection = new Connection(client);
            User user;
            bool success = connection.TryLogin(cmd, obj, out user);

            if(success)
            {
                connection.AcceptLogin(reqId);
                connection.Communicate();
                ConnectedUsers[user.UserID] = user;

                Console.WriteLine("User Logged In:\t" + user.Username);
            }
            else
            {
                connection.RejectLogin(reqId, ClientErrorMessages[cmd]);
            }
        }
        private static bool ReaderStuck(TcpClient client)
        {
            Timer.Restart();

            while (Timer.ElapsedMilliseconds < ConnectionStatics.InbetweenReadTimeout)
            {
                Thread.Sleep(5);
                if (client.GetStream().DataAvailable)
                    return false;
            }

            client.GetStream().Dispose();
            client.Close();
            return true;
        }

        public static Game GetGameForUser(GameTypes gameType)
        {
            Game game;
            List<Game> specificGames = AllGames[gameType];

            lock (specificGames)
            {
                game = specificGames.AsEnumerable()
                                    .FirstOrDefault(g => g.HasOpenSeat());
                if (game == null)
                {
                    switch (gameType)
                    {
                        case GameTypes.Blackjack:
                            game = new Blackjack(NextGameID++);
                            break;
                        case GameTypes.Roulette:
                            //game = new Roulette(NextGameID++);
                            break;
                        case GameTypes.TexasHoldEm:
                            //game = new TexasHoldEm(NextGameID++);
                            break;
                    }
                    game.Start();
                    specificGames.Add(game);
                }
            }
            return game;

        }
        public static SMP.User GetUserInfo(int id)
        {
            if (ConnectedUsers.ContainsKey(id))
                return ConnectedUsers[id].GetSharedModelPublic();

            return null;
        }

        public static void WriteEvent(GameEvent gameEvent)
        {
            Console.WriteLine(Enum.GetName(typeof(BlackjackEvents), (BlackjackEvents)gameEvent.Event));
        }
        public static void WriteException(string throwingMethod, Exception ex)
        {
            Console.WriteLine(String.Format("{0} threw an Exception : {1}", throwingMethod, ex.Message));
        }
        public static void QueueClientError(ServerCommands cmd, string message)
        {
            ClientErrorMessages[cmd] = message;
        }
        
    }
}
