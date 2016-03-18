using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using ServerLogic;
using ServerLogic.Games;
using SharedModels.GameComponents;

namespace ServerLogic
{
    public class ServerMain
    {
        public List<User> ConnectedUsers;
        public List<Game> OpenTables;

        public Dictionary<int, Game> UserIDtoTable;

        public ServerMain()
        {
            User user = new User();
            List<User> users = new List<User>();
            users.Add(user);

            Blackjack bjack = new Blackjack(users);
            bjack.Start();
            Console.Read();

        }

    }
}

