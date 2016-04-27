using SharedModels.Games;
using ServerLogic.Players;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SMG = SharedModels.Games;
using SMP = SharedModels.Players;
using System.Diagnostics;

namespace ServerLogic
{
    public abstract class Game
    {
        public readonly int GameID;
        public readonly int ThreadId;

        protected ConcurrentDictionary<int, Player> Players;
        protected List<Player> SubscribedPlayers;
        private List<Player> _ActivePlayers;
        protected List<Player> ActivePlayers
        {
            get
            {
                lock (ListLock)
                    return _ActivePlayers.OrderBy(p => p.Seat).ToList();
            }

            set
            {
                _ActivePlayers = value;
            }
        }
        protected List<GameEvent> RoundEvents;
        protected GameStates GameState;

        protected SMG.Game RoundSnapshot;
        protected abstract int MaxSeats { get; }
        protected bool EndGame;
        protected Stopwatch sw;

        private ConcurrentDictionary<int, bool> OpenSeats;
        private Thread GameThread;
        private object ListLock;
        

        public Game(int id)
        {
            GameID = id;
            EndGame = false;
            GameState = GameStates.Waiting;

            Players = new ConcurrentDictionary<int, Player>();
            OpenSeats = new ConcurrentDictionary<int, bool>();
            SubscribedPlayers = new List<Player>();
            RoundEvents = new List<GameEvent>();
            
            for (int i = 1; i <= MaxSeats; i++)
                OpenSeats.TryAdd(i, true);

            GameThread = new Thread(() => Run());
            ThreadId = GameThread.ManagedThreadId;
            ListLock = new object();
            sw = new Stopwatch();
        }

        public void Start()
        {
            GameThread.Start();
        }
        protected abstract void Run();

        public abstract Player TakeSeat(User user, int buyIn);        
        public bool HasOpenSeat()
        {
            return OpenSeats.Where(u => u.Value).Any();
        }
        public SMG.Game GetRoundSnapshot()
        {
            return RoundSnapshot;
        }
        public void ShareRoundEvents(Player player)
        {
            lock (ListLock)
            {
                foreach (var e in RoundEvents)
                    player.ShareEvent(e);

                SubscribedPlayers.Add(player);
            }
        }
        public void PlayerQuit(Player player)
        {
            lock (ListLock)
            {
                SubscribedPlayers.Remove(player);
                ActivePlayers.Remove(player);
            }
        }
        protected abstract SMG.Game GetSharedModel();
        protected void Broadcast(GameEvent gameEvent)
        {
            lock (ListLock)
            {
                RoundEvents.Add(gameEvent);

                foreach (var player in SubscribedPlayers)
                        player.ShareEvent(gameEvent);
            }
        }

        protected int TakeNextSeat()
        {
            int seat = -1;

            for(int i = 1; i <= MaxSeats; i++)
            {
                if (OpenSeats[i])
                {
                    OpenSeats[i] = false;
                    seat = i;
                    break;
                }
            }

            return seat;
        }
        protected void ReleaseSeat(int seat)
        {
            OpenSeats[seat] = true;
        }
        protected void ResetRoundVariables()
        {

            lock (Players)
            {
                //Wait if players are joining
                bool playersJoining = true;
                while (playersJoining)
                {
                    playersJoining = Players.AsEnumerable()
                                            .Where(p => p.Value.GetState() == SMP.PlayerStates.Joining)
                                            .Any();
                    Thread.Sleep(5);
                }

                PurgeQuitters();
                RoundEvents.Clear();
                ActivePlayers = Players.Select(p => p.Value).ToList();
                RoundSnapshot = GetSharedModel();

            }
        }

        protected void PurgeQuitters()
        {
            List<int> seats = Players.AsEnumerable()
                                     .Where(p => p.Value.GetState() == SMP.PlayerStates.Quitting)
                                     .Select(p => p.Key)
                                     .ToList();

            foreach(int seat in seats)
            {
                Player player;
                Players.TryRemove(seat, out player);
                ReleaseSeat(seat);
            }
        }
    }

   
   
}
