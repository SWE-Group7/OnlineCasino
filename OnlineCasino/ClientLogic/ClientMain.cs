using ClientLogic.Connections;
using SharedModels;
using SM = SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientLogic.Players;

namespace ClientLogic
{
    public static class ClientMain
    {
        public static User MainUser;
        public static Player MainPlayer;
        public static Connection MainConnection;

        public static bool TrySyncLogin(string username, string password)
        {
            MainConnection = new Connection();
            Security security = new Security(username, password);
            return MainConnection.TrySyncLogin(security, out MainUser);
        }

        public static bool TrySyncLogin(string username, string password, string fullName, string email)
        {
            MainConnection = new Connection();
            Security security = new Security(username, password, fullName, email);
            return MainConnection.TrySyncRegister(security, out MainUser);
        }
    }
}
