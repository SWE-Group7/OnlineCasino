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


            Console.WriteLine("Welcome to the Online Casino!");

            while (user == null)
            {
                               
                Console.WriteLine("Are you a: ");
                Console.WriteLine("\t 1: New User");
                Console.WriteLine("\t 2: Existing User");

                string input;

                input = Console.ReadLine();
                Console.WriteLine();

                if (input == "1")
                {
                    username = Prompt("User Name");
                    password = Prompt("Password");
                    fullName = Prompt("Full Name");
                    email = Prompt("Email Address");

                    user = User.Register(username, password, email, fullName);
                }
                else if(input == "2")
                {
                    username = Prompt("User Name");
                    password = Prompt("Password");

                    user = User.Login(username, password);
                }
                else
                {
                    Console.WriteLine("Error - Invalid Input");
                }
            }

            
            users.Add(user);

            Blackjack bjack = new Blackjack(users);
            bjack.Start();
            Console.Read();
        }

        static string Prompt(string field)
        {
            Console.WriteLine(String.Format("Please enter your {0}", field));
            return Console.ReadLine();
        }
   }
}
