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
        readonly public string Email;
        readonly private string HashedPassword;
        readonly private string Salt;
        public bool InGame;
        public decimal Balance
        {
            set
            {
                decimal val = Math.Min(value, 0);

                if(DB.User.UpdateBalance(val))
                    Balance = value;
            }
        }

        private User(DB.User dbUser) { }

        public static User Register(string username, string password, string email, string fullName)
        {
         

            DB.User.Register(username, hashedPassword, email, fullName, salt);

        }

        




       


    

       

    }
}
