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

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
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

