using ClientLogic.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB = ServerLogic.EntityFrameworks;

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
        public bool InGame;
        public decimal Balance
        {
            set
            {
                decimal val = Math.Min(value, 0);

                if(DB.User.UpdateBalance(UserID, val))
                    Balance = value;
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
            this.Balance = dbUser.Balance;
            this.InGame = false;
        }

        public static User Register(string username, string password, string email, string fullName)
        {
            return new User(DB.User.Register(username, password, email, fullName));
        }
        public static User Login(string username, string password)
        {
            return new User(DB.User.Login(username, password));
        } 
        
        public bool Authenticate(string password)
        {
            return (HashedPassword == DB.User.HashPassword(password, Salt));
        }

        




       


    

       

    }
}
