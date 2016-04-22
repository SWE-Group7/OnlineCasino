using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedModels.Connection;
using System.Runtime.Serialization.Formatters.Binary;
using SharedModels;
using ServerLogic.Connections;


namespace ServerLogic
{
    public class ServerMain
    {
        public List<User> ConnectedUsers;
        public List<Game> OpenTables;
        private TcpListener Listener;

        public ServerMain()
        {
            ConnectedUsers = new List<User>();
            OpenTables = new List<Game>();
            Listener = new TcpListener(IPAddress.Any, 47689);
        }

        public void Start()
        {
            ListenForClients();
        }

        private void ListenForClients()
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            byte[] obj;
            int bytesRead;
            int bytesToRead;
            int index;

            Listener.Start();

            CommType commType;
            ServerCommand cmd;
            int reqId;
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();

                //Get CommType
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType) BitConverter.ToInt32(buffer, 0);

                //If not Request, Close
                if(commType != CommType.Request)
                {
                    client.Close();
                    continue;
                }

                //Get Command
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ServerCommand) BitConverter.ToInt32(buffer, 0);

                //If not Login or Register, Close
                if (!(cmd == ServerCommand.Login || cmd == ServerCommand.Register))
                {
                    client.Close();
                    continue;
                }

                //Get Request ID and Object Length
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                reqId = BitConverter.ToInt32(buffer, 0);
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                //Get Object
                obj = new byte[bytesToRead];
                index = 0;
                while (bytesToRead > 0)
                {   
                    bytesRead = client.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
                    Array.ConstrainedCopy(buffer, 0, obj, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }

                Thread thread = new Thread(() => Login(client, cmd, reqId, obj));
                thread.Start();

            }
        }

        private void Login(TcpClient client, ServerCommand cmd, int reqId, byte[] obj)
        {
            Connection connection = new Connection(client);
            User user;
            bool success = connection.TryLogin(cmd, obj, out user);

            if(success)
            {
                connection.AcceptLogin(reqId);
                connection.Communicate();

                lock (ConnectedUsers)
                    ConnectedUsers.Add(user);
            }
            else
            {
                connection.RejectLogin(reqId);
                
            }
        }

        static public void  WriteException(string throwingMethod, Exception ex)
        {
            Console.WriteLine(String.Format("{0} threw an Exception : {1}", throwingMethod, ex.Message));
        }

        
    }
}
