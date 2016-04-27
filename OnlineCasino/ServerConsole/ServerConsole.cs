//using ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole //print statements & request
{
    class Program
    {
        static void Main(string[] args)
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

            for (int i = 0; i < 30; i++)
            {
                byte[] bytes = new byte[32];
                random.GetNonZeroBytes(bytes);
                Console.WriteLine(Convert.ToBase64String(bytes));
            }
            
            //ServerMain.Start();
        }
    }
}
