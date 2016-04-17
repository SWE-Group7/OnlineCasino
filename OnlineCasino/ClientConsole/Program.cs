using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientLogic.Connections;
using SharedModels.Players;

namespace ClientConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            Connection connection = new Connection();
            string username = Prompt("Username:");
            string password = Prompt("Password:");

            User user = connection.SyncLogin(username, password);
            
            
        }

        private static string Prompt(string str)
        {
            Console.WriteLine(str);
            return Console.ReadLine();
        }
    }
}
