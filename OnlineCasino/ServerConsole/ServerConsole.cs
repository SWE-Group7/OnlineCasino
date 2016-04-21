using ServerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole //print statements & request
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerMain Server = new ServerMain();
            
            Server.Start();
            
            
        }
    }
}
