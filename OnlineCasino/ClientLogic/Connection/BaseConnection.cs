using SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Connection
{
    public class BaseConnection
    {
        TcpClient server;

        public BaseConnection(string username, string password, string FullName, string email)
        {
            server.Connect("127.0.0.1", 47689);
        }

        public User Login(string username, string password)
        {
            return null;
        }

        public bool CheckConnection()
        {
            return false;
        }

        public void IndicateWaiting()
        {

        }

        public void IndicateBetting()
        {

        }

        public void IndicatePlaying()
        {

        }
    }
}
