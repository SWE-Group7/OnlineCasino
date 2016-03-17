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
            
            Console.Write("Starting Server..");

            //while(1)?
            //Write to console every time client connects
            //Server.ConnectedUsers.Add(User)
            Console.Write(" -UserID- -UserName- has connected.");

            //write to console every time a game start
            //Server.OpenTables.Add(Game)
            Console.Write(" New table starting -Game- with players: -User- -User- ");

            //write to console every time a game ends
            //Server.OpenTables.Remove(Game)
            Console.Write(" Table -Game- has closed.");

            //write to console every time a client disconnects
            //Server.ConnectedUsers.Remove(User)
            Console.Write(" -UserID- -Username- has disconnected. ");

            Console.Write(Server.ConnectedUsers.Count + " connected users : " + Server.OpenTables.Count + " tables open");
        }
    }
}
