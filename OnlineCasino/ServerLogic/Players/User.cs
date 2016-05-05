using ServerLogic.Connections;
using ServerLogic.Players;
using SharedModels.Connection.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB = ServerLogic.EntityFrameworks;
using SM = SharedModels;
using SMG = SharedModels.Games;
using SharedModels.Connection;
using SharedModels.Games.Events;

namespace ServerLogic
{
    public class User
    {
        readonly public int UserID;
        readonly public string Username;
        readonly public string FullName;
        readonly public string EmailAddress;
        readonly private string HashedPassword;
        readonly private string Salt;
        public int GameID;
        public bool InGame;
        public bool Connected;

        private Connection CurrentConnection;
        private UserCache Cache;
        private object Lock;
        private Player CurrentPlayer;
        private Game CurrentGame;

        private decimal balance;
        public decimal Balance
        {
            get
            {
                lock(Lock)
                    return balance;
            }
        }

        private User(DB.User dbUser, Connection connection)
        {
            this.UserID = dbUser.UserID;
            this.Username = dbUser.Username;
            this.FullName = dbUser.FullName;
            this.EmailAddress = dbUser.EmailAddress;
            this.HashedPassword = dbUser.Password;
            this.Salt = dbUser.Salt;
            this.balance = dbUser.Balance;
            this.InGame = false;
            this.Connected = true;
            this.Lock = new object();
            this.CurrentConnection = connection;

            Cache = new UserCache();
        }

        public static User Register(string username, string password, string email, string fullName, Connection connection)
        {
            DB.User dbUser = DB.User.Register(username, password, email, fullName);

            if (dbUser != null)
                return new User(dbUser, connection);
            else
                return null;
        }
        public static User Login(string username, string password, Connection connection)
        {
            DB.User dbUser = DB.User.Login(username, password);

            if (dbUser != null)
                return new User(dbUser, connection);
            else
                return null;
        }
        public bool Authenticate(string password)
        {
            return (HashedPassword == DB.User.HashPassword(password, Salt));
        }

        public void JoinGame(GameTypes gameType, int buyIn, int reqId)
        {
            CurrentPlayer = null;

            while (CurrentPlayer == null)
            {
                CurrentGame = ServerMain.GetGameForUser(gameType);
                CurrentPlayer = CurrentGame.TakeSeat(this, buyIn);
            }
            InGame = true;
            SMG.Game smGame = CurrentGame.GetRoundSnapshot();
            CurrentConnection.Return(reqId, true, smGame);

            CurrentGame.ShareRoundEvents(CurrentPlayer);

            CurrentPlayer.FinalizeJoin();
            GameID = CurrentGame.GameID;

        }

        public SM.RequestResult Request(ClientCommands cmd)
        {
            return CurrentConnection.Request(cmd);
        }

        public void AddToBalance(decimal value)
        {
            lock (Lock)
            {
                balance += value;
                balance = Math.Max(balance, 0);
                value = balance;
            }

            DB.User.UpdateBalance(UserID, value);
        }
        public void ShareEvent(GameEvent gameEvent)
        {
            if (InGame)
                CurrentConnection.Command(ClientCommands.SendEvent, gameEvent);
        }
        public void FinalizeQuit()
        {
            GameID = 0;
            InGame = false;

            ServerMain.UserQuit(this.UserID);
        }
        public void ForceQuit()
        {
            InGame = false;
            CurrentConnection.Command(ClientCommands.ForceQuit);
        }

        public void ClientQuitGame(bool hardQuit)
        {
            if (hardQuit)
                InGame = false;

            CurrentPlayer.UserQuit(hardQuit);
        }
        public void ClientDisconnect()
        {
            if (InGame)
                CurrentPlayer.UserQuit(true);
                
            InGame = false;
            Connected = false;
        }

        public SM.Players.User GetSharedModelPrivate()
        {
            if (Cache.Old)
            {
                lock (Lock)
                    Cache.smUserPrivate = new SM.Players.User(UserID, Username, FullName, EmailAddress, balance);
            }

            return Cache.smUserPrivate;
        }
        public SM.Players.User GetSharedModelPublic()
        {
            if (Cache.Old)
            {
                lock (Lock)
                    Cache.smUser = new SM.Players.User(UserID, Username, FullName);
            }
            return Cache.smUser;
        }
        private class UserCache
        {
            internal SM.Players.User smUser;
            internal SM.Players.User smUserPrivate;
            internal volatile bool Old = true;
        }

        
    }
}
