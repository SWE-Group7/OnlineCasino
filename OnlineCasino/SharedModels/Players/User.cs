using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    [Serializable]
    public class User
    {
        readonly public int UserID;
        readonly public string Username;
        readonly public string FullName;
        readonly public string Email;
        readonly public decimal Balance;
        readonly public bool Online;
        readonly public bool InGame;
        readonly public int GameID;
        readonly DateTime LastLog;

        public User(int uID, string username, string fullname, string email, decimal balance, bool online, bool inGame, int gameID, DateTime lastLog)
        {
            UserID = uID;
            Username = username;
            FullName = fullname;
            Email = email;
            Balance = balance;
            Online = online;
            InGame = inGame;
            GameID = gameID;
            LastLog = lastLog;
        }

        public User(int uID, string username, string fullname, string email, decimal balance)
        {
            UserID = uID;
            Username = username;
            FullName = fullname;
            Email = email;
            Balance = balance;
        }

        public User(int uID, string username, string fullName)
        {
            UserID = uID;
            Username = username;
            FullName = fullName;
        }


        
    }
}
