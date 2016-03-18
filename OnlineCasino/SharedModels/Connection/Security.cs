using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class Security
    {
        string Username;
        string Password;

        public Security(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }  
        
    }
}
