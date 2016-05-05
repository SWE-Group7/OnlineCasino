using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic;
using ServerLogic.Games;
using SharedModels.GameComponents;

namespace RoulTest
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = null;
            List<User> users = new List<User>();
            bool isDec = false;
            decimal balance = 0;

            while (isDec == false)
            {
                Console.WriteLine("What is your balance? (must be number)\n");
                string bal = Console.ReadLine();
                isDec = decimal.TryParse(bal, out balance);
            }
            
            user = new User(balance);
            users.Add(user);

            Console.WriteLine("Let's play Roulette!\n");
            Roulette roul = new Roulette(users);
            roul.Start();
            Console.Read();

        }
    }
}
