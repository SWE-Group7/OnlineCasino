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

            ServerCommand cmd;
            while (true)
            {
                TcpClient client = Listener.AcceptTcpClient();
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ServerCommand) BitConverter.ToInt32(buffer, 0);

                if (!(cmd == ServerCommand.Login || cmd == ServerCommand.Login))
                    continue;

                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                obj = new byte[bytesToRead];
                index = 0;
                while (bytesToRead > 0)
                {   
                    bytesRead = client.GetStream().Read(buffer, 0, bytesToRead % bufferSize);
                    Array.ConstrainedCopy(buffer, 0, obj, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }
                
                switch (cmd)
                {
                    case ServerCommand.Login:
                        Security security = (Security) Serializer.Deserialize(obj);
                        User user = User.Login(security.Username, security.Password);
                        
                        
                        break;
                    case ServerCommand.Register:
                        break;

                }

                
                
            }
        }

        static public void  WriteException(string throwingMethod, Exception ex)
        {
            Console.WriteLine(String.Format("{0} threw an Exception : {1}", throwingMethod, ex.Message));
        }


    }
}
