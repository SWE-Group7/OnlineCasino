using ServerLogic.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB = ServerLogic.EntityFrameworks;
using SM = SharedModels;


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
        public Connection Connection;
        
        public bool InGame;

        private decimal balance;
        public decimal Balance
        {
            get { return balance; }
            set
            {
                decimal val = Math.Min(value, 0);

                if(DB.User.UpdateBalance(UserID, val))
                    balance = value;
            }
        }

        private User(DB.User dbUser)
        {
            this.UserID = dbUser.UserID;
            this.Username = dbUser.Username;
            this.FullName = dbUser.FullName;
            this.EmailAddress = dbUser.EmailAddress;
            this.HashedPassword = dbUser.Password;
            this.Salt = dbUser.Salt;
            this.balance = dbUser.Balance;
            this.InGame = false;
        }

        public static User Register(string username, string password, string email, string fullName)
        {
            DB.User dbUser = DB.User.Register(username, password, email, fullName);

            if (dbUser != null)
                return new User(dbUser);
            else
                return null;
        }
        public static User Login(string username, string password)
        {
            DB.User dbUser = DB.User.Login(username, password);

            if (dbUser != null)
                return new User(dbUser);
            else
                return null;
        }
        
        public SM.Players.User GetSharedModelPrivate()
        {
            return new SM.Players.User(UserID, Username, FullName, EmailAddress, Balance);
        } 
        
        public bool Authenticate(string password)
        {
            return (HashedPassword == DB.User.HashPassword(password, Salt));
        }

        




       


    

       

    }
}
