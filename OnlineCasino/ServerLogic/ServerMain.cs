using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public class ServerMain
    {
        public List<User> ConnectedUsers;
        public List<Game> OpenTables;

        public Dictionary<int, Game> UserIDtoTable;

        public ServerMain()
        {
            ConnectedUsers = new List<User>();
            OpenTables = new List<Game>();

            UserIDtoTable = new Dictionary<int, Game>();
        }

        static public void  WriteException(string throwingMethod, Exception ex)
        {
            Console.WriteLine(String.Format("{0} threw an Exception : {1}", throwingMethod, ex.Message));
        }
    }


}
