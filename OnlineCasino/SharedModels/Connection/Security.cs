using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    [Serializable]
    public class Security
    {
        public readonly string Username;
        public readonly string Password;
        public readonly string FullName;
        public readonly string EmailAddress;

        public Security(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }  

        public Security(string username, string password, string fullName, string email)
        {
            this.Username = username;
            this.Password = password;
            this.FullName = fullName;
            this.EmailAddress = email;
        }
        
    }
}
