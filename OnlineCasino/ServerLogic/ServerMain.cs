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
using System.Diagnostics;

namespace ServerLogic
{
    public static class ServerMain
    {
        public static List<User> ConnectedUsers;
        public static List<Game> OpenTables;
        private static TcpListener Listener;
        private static Stopwatch Timer = new Stopwatch();

        public static void Start()
        {
            ConnectedUsers = new List<User>();
            OpenTables = new List<Game>();
            Listener = new TcpListener(IPAddress.Any, 47689);
            ListenForClients();
        }

        private static void ListenForClients()
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

                //Ensure not stuck
                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                //Ensure Command Start
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType)BitConverter.ToInt32(buffer, 0);
                if (commType != CommType.Start)
                    continue;

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;
                    
                //Get CommType
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType) BitConverter.ToInt32(buffer, 0);

                //If not Request, Close
                if(commType != CommType.Request)
                {
                    client.GetStream().Dispose();
                    client.Close();
                    continue;
                }

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                //Get Command
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ServerCommand) BitConverter.ToInt32(buffer, 0);

                //If not Login or Register, Close
                if (!(cmd == ServerCommand.Login || cmd == ServerCommand.Register))
                {
                    client.GetStream().Dispose();
                    client.Close();
                    continue;
                }

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                //Get Request ID and Object Length
                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                reqId = BitConverter.ToInt32(buffer, 0);

                if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    continue;

                bytesRead = client.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                //Get Object
                obj = new byte[bytesToRead];
                index = 0;
                bool stuck = false;
                while (bytesToRead > 0)
                {
                    if (!client.GetStream().DataAvailable && ReaderStuck(client))
                    {
                        stuck = true;
                        break;
                    }

                    bytesRead = client.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
                    Array.ConstrainedCopy(buffer, 0, obj, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }

                if (stuck)
                    continue;

                Thread thread = new Thread(() => Login(client, cmd, reqId, obj));
                thread.Start();

            }
        }

        private static void Login(TcpClient client, ServerCommand cmd, int reqId, byte[] obj)
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

        private static bool ReaderStuck(TcpClient client)
        {
            Timer.Restart();

            while (Timer.ElapsedMilliseconds < ConnectionStatics.InbetweenReadTimeout)
            {
                Thread.Sleep(5);
                if (client.GetStream().DataAvailable)
                    return false;
            }

            client.GetStream().Dispose();
            client.Close();
            return true;
        }

        static public void  WriteException(string throwingMethod, Exception ex)
        {
            Console.WriteLine(String.Format("{0} threw an Exception : {1}", throwingMethod, ex.Message));
        }

        
    }
}
