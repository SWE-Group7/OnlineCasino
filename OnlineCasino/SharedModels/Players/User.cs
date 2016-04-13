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
        readonly public decimal Balance;
        

        public User(int uID, string username, string fullname, string email, decimal balance)
        {
            UserID = uID;
            Username = username;
            FullName = fullname;
            EmailAddress = email;
            Balance = balance;
        }


        
    }
}
