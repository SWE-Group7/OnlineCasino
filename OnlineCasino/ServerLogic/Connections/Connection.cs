using SharedModels;
using SM = SharedModels.Players;
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
        private int RequestId = 1;

        private Thread Reader;
        private Thread Writer;

        private Queue<Payload> WriteQueue;
        private volatile bool WriteQueueEmpty;

        private Queue<Payload> ReadQueue;
        private Dictionary<int, RequestResult> RequestedByServer;
        private Dictionary<ServerCommand, AsyncBuffer> Buffers;

        private TcpClient Client;
        private User CurrentUser;
        private bool Disconnect;

        public Connection(TcpClient client)
        {
            this.Client = client;
            Disconnect = false;
            Connected = false;

            WriteQueue = new Queue<Payload>();
            ReadQueue = new Queue<Payload>();
        }

        public bool TryLogin(ServerCommand cmd, byte[] obj, out User userOut)
        {
            try
            {
                Security security = (Security)Serializer.Deserialize(obj);

                if (cmd == ServerCommand.Login)
                    CurrentUser = User.Login(security.Username, security.Password);
                else if (cmd == ServerCommand.Register)
                    CurrentUser = User.Register(security.Username, security.Password, security.EmailAddress, security.FullName);

                userOut = CurrentUser;
                return (CurrentUser != null);
               
            }
            catch (Exception ex)
            {
                ServerMain.WriteException("Connection.TryLogin(cmd, obj)", ex);
                userOut = null;
                return false;
            }
        }

        public void AcceptLogin(int reqId)
        {
            SM.User smUser;
            lock (CurrentUser)
                smUser = CurrentUser.GetSharedModelPrivate();

            ImmediateWrite(new Payload(CommType.Return, reqId, true, smUser));
        }

        public void RejectLogin(int reqId)
        {
            ImmediateWrite(new Payload(CommType.Return, reqId, false, null));
        }

        public void Communicate()
        {
            Reader = Thread.CurrentThread;
            Writer = new Thread(() => StartWriter());
            Writer.Start();
            StartReader();
        }

        public RequestResult Request(ClientCommand cmd, object obj = null)
        {
            RequestResult ret = new RequestResult();
            int reqId = Interlocked.Increment(ref RequestId);

            lock (RequestedByServer)
            {
                RequestedByServer.Add(reqId, ret);
            }

            Payload payload = new Payload(CommType.Request, cmd, reqId, obj);
            QueueWrite(payload);

            return ret;
        }

        private void ReturnSuccess(int reqId, object obj)
        {
            Payload payload = new Payload(CommType.Return, reqId, true, obj);
            QueueWrite(payload);   
        }

        private void ReturnFailure(int reqId)
        {
            Payload payload = new Payload(CommType.Return, reqId, false, null);
            QueueWrite(payload);
        }

        public void Command(ClientCommand cmd, object obj = null)
        {
            //Payload payload = new Payload((int)cmd, obj);
            //QueueWrite(payload);
        }

        private void QueueWrite(Payload payload)
        {
            lock (WriteQueue)
            {
                WriteQueue.Enqueue(payload);
                WriteQueueEmpty = false;
            }

        }

        private void StartReader()
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            while (!Disconnect)
            {
                CommType commType;
                ServerCommand cmd = 0;
                int reqId = -1;

                byte[] obj;
                int index;
                int bytesRead;
                int bytesToRead;

                //Get Comm Type
                bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType)BitConverter.ToInt32(buffer, 0);

                //Get CommType-specific data
                switch (commType)
                {
                    case CommType.Void:
                        bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                        cmd = (ServerCommand)BitConverter.ToInt32(buffer, 0);
                        break;
                    case CommType.Request:
                        bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                        cmd = (ServerCommand)BitConverter.ToInt32(buffer, 0);

                        bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                        reqId = BitConverter.ToInt32(buffer, 0);
                        break;
                    case CommType.Return:
                        bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                        reqId = BitConverter.ToInt32(buffer, 0);
                        break;
                    default:
                        break;
                }

                //Get Object Length
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

                //Handle
                switch (commType)
                {
                    case CommType.Return:
                        HandleReturn(reqId, obj);
                        break;
                    case CommType.Request:
                        HandleRequest(cmd, reqId, obj);
                        break;
                    case CommType.Void:
                        HandleVoid(cmd, obj);
                        break;
                    default:
                        break;

                }
            }
        }

        private void HandleVoid(ServerCommand cmd, byte[] obj)
        {
            throw new NotImplementedException();
        }

        private void HandleRequest(ServerCommand cmd, int reqId, byte[] obj)
        {
            throw new NotImplementedException();
        }

        private void HandleReturn(int reqId, byte[] objBytes)
        {
            RequestResult result;
            lock (RequestedByServer)
            {
                if (RequestedByServer.TryGetValue(reqId, out result))
                    RequestedByServer.Remove(reqId);
            }

            if(result != null)
            {
                object obj = Serializer.Deserialize(objBytes);
                result.SetValue(obj);
            }
        }

        private void StartWriter()
        {
            while (Connected)
            {
                //Wait if queue is empty
                while (WriteQueueEmpty)
                    Thread.Sleep(1);

                Payload next;
                lock (WriteQueue)
                {
                    next = WriteQueue.Dequeue();
                    WriteQueueEmpty = !WriteQueue.Any();
                }

                ImmediateWrite(next);

            }
        }

        private bool ImmediateWrite(Payload payload)
        {
            CommType type = payload.Type;
            ClientCommand cmd = (ClientCommand) payload.Command;
            int reqId = payload.RequestId;
            object obj = payload.Object;

            try
            {
                //Write CommType
                byte[] byteType = BitConverter.GetBytes((int)type);
                Client.GetStream().Write(byteType, 0, sizeof(int));

                //Write Command or 0/1 for return
                byte[] byteCmd = BitConverter.GetBytes((int)cmd);
                Client.GetStream().Write(byteCmd, 0, sizeof(int));
                
                //Write RequestId or 0 for void commands
                byte[] byteId = BitConverter.GetBytes(reqId);
                Client.GetStream().Write(byteId, 0, sizeof(int));
                
                //Write object length and object
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
