using SM = SharedModels;
using SharedModels.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ServerLogic.Connections
{
    public class Connection
    {

        public bool Connected;
        public long LastComm;
        private ulong PayloadId = 2;

        private Thread Reader;
        private Thread Writer;

        private Queue<Tuple<ClientCommand, object>> WriteQueue;
        private Queue<Tuple<ServerCommand, object>> ReadQueue;
        
        
        private TcpClient Client;
        private User CurrentUser;
        private bool Disconnect;

        public Connection(TcpClient client)
        {
            this.Client = client;
            Disconnect = false;
            Connected = false;

            WriteQueue = new Queue<Tuple<ClientCommand, object>>();
            ReadQueue = new Queue<Tuple<ServerCommand, object>>();
        }

        public User TryLogin(ServerCommand cmd, byte[] obj)
        {
            try
            {
                SM.Security security = (SM.Security)Serializer.Deserialize(obj);

                if (cmd == ServerCommand.Login)
                    CurrentUser = User.Login(security.Username, security.Password);
                else if (cmd == ServerCommand.Register)
                    CurrentUser = User.Register(security.Username, security.Password, security.EmailAddress, security.FullName);

                return CurrentUser;
            }
            catch (Exception ex)
            {
                ServerMain.WriteException("Connection.TryLogin(cmd, obj)", ex);
                return null;
            }
        }

        public void AcceptLogin()
        {
            SM.Players.User smUser;

            lock(CurrentUser)
                smUser = CurrentUser.GetSharedModel();

            ImmediateWrite(ClientCommand.AcceptLogin, smUser);

        }

        public void RejectLogin()
        {
          //  ImmediateWrite(ClientCommand.RejectLogin);
        }

        public void Communicate()
        {
            Reader = Thread.CurrentThread;
            Writer = new Thread(() => StartWriter());
            Writer.Start();
            StartReader();
        }

        public void Write(ClientCommand cmd, object obj)
        {
            lock(WriteQueue)
                WriteQueue.Enqueue(new Tuple<ClientCommand, object>(cmd, obj));
        }

        private void StartReader()
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            while (!Disconnect)
            {
                ServerCommand cmd;
                byte[] obj;
                int index;
                int bytesRead;
                int bytesToRead;

                //Get Command and Object Size
                bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ServerCommand)BitConverter.ToInt32(buffer, 0);
                Client.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                //Get object bytes
                obj = new byte[bytesToRead];
                index = 0;
                while (bytesToRead > 0)
                {
                    bytesRead = Client.GetStream().Read(buffer, 0,  Math.Min(bufferSize, bytesToRead));
                    Array.ConstrainedCopy(buffer, 0, obj, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }

                switch (cmd)
                {
                    case ServerCommand.Disconnect:
                        this.Disconnect = true;
                        break;
                    case ServerCommand.GetUser:
                        this.Write(ClientCommand.ReturnUser, CurrentUser.GetSharedModel());
                        break;


                }
            }

            
            if (Connected)
            {
                Connected = false;
                

                ImmediateWrite(ClientCommand.Disconnect);
            }
        }

        private void StartWriter()
        {
            while (Connected)
            {
                //Wait until there is something in queue
                while (!WriteQueue.Any())
                    Thread.Sleep(1);

                var next = WriteQueue.Dequeue();
                ClientCommand cmd = (ClientCommand)next.Item1;
                object obj = next.Item2;

                ImmediateWrite(cmd, obj);

            }
        }

        private bool ImmediateWrite(ClientCommand cmd)
        {
            try
            {
                byte[] byteCmd = BitConverter.GetBytes((int)cmd);
                Client.GetStream().Write(byteCmd, 0, sizeof(int));
                Client.GetStream().Write(BitConverter.GetBytes(0), 0, sizeof(int));
                return true;
            }
            catch (Exception ex)
            {
                ServerMain.WriteException("Connection.ImmediateWrite(cmd)", ex);
                return false;
            }
        }

        private bool ImmediateWrite(ClientCommand cmd, object obj)
        {
            try
            {                
                byte[] byteCmd = BitConverter.GetBytes((int) cmd);
                Client.GetStream().Write(byteCmd, 0, sizeof(int));

                byte[] byteData = Serializer.Serialize(obj);
                byte[] length = BitConverter.GetBytes(byteData.Length);

                Client.GetStream().Write(length, 0, sizeof(int));
                Client.GetStream().Write(byteData, 0, byteData.Length);
                
                return true;
            }
            catch (Exception ex)
            {
                ServerMain.WriteException("Connection.ImmediateWrite(cmd, obj)", ex);
                return false;
            }
        }

    }
}
