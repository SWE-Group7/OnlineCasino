using SharedModels;
using SharedModels.Connection;
using SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Connections
{
    public class Connection
    {
        TcpClient server;
        const int bufferSize = 4048;

        public Connection()
        {
            server = new TcpClient("127.0.0.1", 47689);
        }

        public User SyncLogin(string username, string password)
        {
            User user;
            Security security = new Security(username, password);
            ImmediateWrite(ServerCommand.Login, security);

            //Temp comment

            object retObj = null;
            ClientCommand command = BlockingRead(out retObj);

            switch (command)
            {
                case ClientCommand.LoginSuccess:
                    user = (User) retObj;
                    break;
                case ClientCommand.LoginFail:
                default:
                    user = null;
                    break; 
            }

            return user;
        }

        public User SyncRegister(string username, string password, string fullName, string email)
        {
            Security security = new Security(username, password, fullName, email);
            ImmediateWrite(ServerCommand.Register, security);

            object retObj = null;
            ClientCommand command = BlockingRead(out retObj);
            User user = null;
            switch (command)
            {
                case ClientCommand.LoginSuccess:
                    user = (User)retObj;
                    break;
                case ClientCommand.LoginFail:
                default:
                    user = null;
                    break;
            }

            return user;
        }

        private void ImmediateWrite(ServerCommand cmd, object obj)
        {
            try {
                byte[] cmdBytes = BitConverter.GetBytes((int)cmd);
                server.GetStream().Write(cmdBytes, 0, sizeof(int));

                byte[] objBytes = Serializer.Serialize(obj);
                byte[] length = BitConverter.GetBytes(objBytes.Length);

                server.GetStream().Write(length, 0, sizeof(int));
                server.GetStream().Write(objBytes, 0, objBytes.Length);
            } catch (Exception ex)
            {
                Console.WriteLine("Exception occured in Connection.ImmediateWrite");
            }
        }

        private ClientCommand BlockingRead(out object retObj)
        {
            //Blocking Read
            ClientCommand command;
            byte[] buffer = new byte[bufferSize];
            byte[] objBytes;
            int index;
            int bytesRead;
            int bytesToRead;

            //Get Command and byte[] Size
            bytesRead = server.GetStream().Read(buffer, 0, sizeof(int));
            command = (ClientCommand)BitConverter.ToInt32(buffer, 0);
            server.GetStream().Read(buffer, 0, sizeof(int));
            bytesToRead = BitConverter.ToInt32(buffer, 0);

            //Get object bytes
            objBytes = new byte[bytesToRead];
            index = 0;
            while (bytesToRead > 0)
            {
                bytesRead = server.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
                Array.ConstrainedCopy(buffer, 0, objBytes, index, bytesRead);
                index += bytesRead;
                bytesToRead -= bytesRead;
            }

            if (objBytes.Any())
            {
                retObj = Serializer.Deserialize(objBytes);
            }
            else
            {
                retObj = null;
            }

            return command;
        }

        
    }
}
