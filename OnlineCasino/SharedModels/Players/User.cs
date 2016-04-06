using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
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

        private User(int uID, string username, string fullname, string email)
        {
            // user sign in
        }

        public static bool Register(string username, string password, string email, string fullName)
        {
            // user registration (false if unable to register)
            return false;
        }
    }
}
