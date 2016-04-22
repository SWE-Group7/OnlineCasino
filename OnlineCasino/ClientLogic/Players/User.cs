using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM = SharedModels.Players;

namespace ClientLogic.Players
{
    public class User
    {
        public int UserID;
        public string Username;
        public string FullName;
        public string Email;
        
        public decimal Balance;

        public User(SM.User smUser)
        {
            this.UserID = smUser.UserID;
            this.Username = smUser.Username;
            this.FullName = smUser.FullName;
            this.Email = smUser.Email;
            this.Balance = smUser.Balance;
        }
    }
}
