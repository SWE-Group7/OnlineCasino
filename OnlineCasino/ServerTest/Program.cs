using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic;
using ServerLogic.Games;
using SharedModels.GameComponents;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = null;
            List<User> users = new List<User>();
            string password;
            string username;
            string fullName;
            string email;
            string input1;
            string input2;
            bool addAnotherUser = false;

            Console.WriteLine("Welcome to the Online Casino!");

            while (user == null || addAnotherUser)
            {
                bool successful = true;
                Console.WriteLine("Are you a: ");
                Console.WriteLine("\t 1: New User");
                Console.WriteLine("\t 2: Existing User");

                input1 = Console.ReadLine();
                Console.WriteLine();

                switch(input1)
                {
                    case "1":                  
                        username = Prompt("User Name");
                        password = Prompt("Password");
                        fullName = Prompt("Full Name");
                        email = Prompt("Email Address");

                        user = User.Register(username, password, email, fullName);
                        if (user == null) Console.WriteLine("Username taken. Try Again:");
                        else users.Add(user);
                        break;
                    case "2":                  
                        username = Prompt("User Name");
                        password = Prompt("Password");

                        user = User.Login(username, password);
                        if (user == null) Console.WriteLine("Username/Password combination does not match. Try Again:");
                        else users.Add(user);
                        break;
                    default:                    
                        Console.WriteLine("Error - Invalid Input");
                        successful = false;
                        break;
                }

                addAnotherUser = false;
                while ((user != null) && (!addAnotherUser))
                {
                    Console.WriteLine("Do you want to..");
                    Console.WriteLine("\t 1: Add Another User");
                    Console.WriteLine("\t 2: Play Blackjack");
                    Console.WriteLine("\t 3: Play Roulette");
                    input2 = Console.ReadLine();
                    Console.WriteLine();

                    switch (input2)
                    {
                        case "1":
                            addAnotherUser = true;
                            break;
                        case "2":
                            Blackjack bjack = new Blackjack(users);
                            bjack.Start();
                            Console.Read();
                            break;
                        case "3":
                            Roulette roulette = new Roulette(users);
                            roulette.Start();
                            Console.Read();
                            break;
                        default:
                            Console.WriteLine("Error - Invalid Input");
                            break;

                    }
                }
            }
        }

        static string Prompt(string field)
        {
            Console.WriteLine(String.Format("Please enter your {0}", field));
            return Console.ReadLine();
        }
   }
}
