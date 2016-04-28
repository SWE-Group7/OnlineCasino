using ServerLogic.Players;
using SharedModels.Connection.Enums;
using SMP = SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedModels.Games;
using SharedModels;
using SharedModels.Connection;
using System.Collections.Concurrent;
using SharedModels.Games.Enums;

namespace ServerLogic.Players
{
    public abstract class Player
    {
        public readonly int Seat;
        protected readonly Game CurrentGame;
        protected readonly User CurrentUser;
        protected readonly int BuyIn;
        protected readonly int GameThreadId;
        protected int State;

        protected int GameBalance;
        protected int Bet;
        protected object Lock;

        protected ConcurrentDictionary<ClientCommands, RequestResult> Results;
        protected Player(User user, Game game, int seat, int buyIn)
        {
            //base class. Abstract
            CurrentUser = user;
            CurrentGame = game;
            GameThreadId = game.ThreadId;
            Seat = seat;
            BuyIn = buyIn;
            GameBalance = buyIn;
            State = (int)SMP.PlayerStates.Joining;
            Results = new ConcurrentDictionary<ClientCommands, RequestResult>();
        }

        
        public void ShareEvent(GameEvent gameEvent)
        {
            CurrentUser.ShareEvent(gameEvent);
        }

        public SMP.Player GetSharedModel()
        {
            lock (Lock)
                return GetSharedModelOR();
        }
        public int GetGameBalance()
        {
            if (GameThreadId == Thread.CurrentThread.ManagedThreadId)
                return GameBalance;
            else
                return -1;
        }
        public int UpdateGameBalance(decimal betMultiplier)
        {
            if (GameThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                GameBalance += (int) Math.Round(Bet * betMultiplier);
                GameBalance = Math.Max(GameBalance, 0);
                return GameBalance;
            }
                
            return -1;
        }
        public void SetBet(int bet)
        {
            if (GameThreadId == Thread.CurrentThread.ManagedThreadId)
                Interlocked.Exchange(ref Bet, bet);
        }
        public SMP.PlayerStates GetState()
        {
            return (SMP.PlayerStates)State;
        }
        public void UserQuit(bool hardQuit)
        {
            ChangeState(SMP.PlayerStates.Quitting);

            if (hardQuit)
                CurrentGame.PlayerQuit(this);

        }
        public void FinalizeJoin()
        {
            ChangeState(SMP.PlayerStates.Playing);
        }

        public void ForceCashOut()
        {
            if (Thread.CurrentThread.ManagedThreadId == GameThreadId)
            {
                ChangeState(SMP.PlayerStates.Quitting);
                CurrentUser.AddToBalance(GameBalance - BuyIn);
                CurrentUser.ForceQuit();
            }
            
        }
        public void FinalizeQuit()
        {
            if (Thread.CurrentThread.ManagedThreadId == GameThreadId)
            {
                CurrentUser.AddToBalance(GameBalance - BuyIn);
                CurrentUser.FinalizeQuit();
            }
            
        }
        public void Request(ClientCommands cmd)
        {
            Results[cmd] = CurrentUser.Request(cmd);
        }
        public bool TryGetResult<T>(ClientCommands cmd, out T obj)
        {
            return Results[cmd].GetValue<T>(out obj);
        }

        protected virtual SMP.Player GetSharedModelOR()
        {

            SMP.User smUser;
            smUser = CurrentUser.GetSharedModelPublic();
            return new SMP.Player(smUser, Seat, GameBalance);
            
        }

        private void ChangeState(SMP.PlayerStates state)
        {
            Interlocked.Exchange(ref State, (int)state);
        }
    }


}
