using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientLogic.Connections;
using SharedModels.Players;
using ClientLogic;

namespace ClientConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            string username = Prompt("Username:");
            string password = Prompt("Password:");

            ClientMain.TrySyncLogin(username, password);
            
        }

        private static string Prompt(string str)
        {
            Console.WriteLine(str);
            return Console.ReadLine();
        }
    }
}
 