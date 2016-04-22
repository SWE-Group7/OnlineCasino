using SharedModels;
using SharedModels.Connection;
using SM = SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientLogic.Players;

namespace ClientLogic.Connections
{
    public class Connection
    {
        private TcpClient Server;
        private const int bufferSize = 4096;

        public bool Connected;
        public long LastRead;
        public long LastWrite;
        private int RequestId = 1;

        private Thread Reader;
        private Thread Writer;

        private Queue<Payload> WriteQueue;
        private volatile bool WriteQueueEmpty;
        private Dictionary<int, RequestResult> RequestedByServer;

        public Connection()
        {
            Server = new TcpClient("127.0.0.1", 47689);
            Connected = true;
            WriteQueueEmpty = true;

            Reader = Thread.CurrentThread;
            Writer = new Thread(() => StartWriter());
            Writer.Start();
            Reader = new Thread(() => StartReader());
            Reader.Start();

        }

        public bool TrySyncLogin(Security security, out User user)
        {
            user = SyncLogin(ServerCommand.Login, security);
            return (user != null);
        }

        public bool TrySyncRegister(Security security, out User user)
        {
            user = SyncLogin(ServerCommand.Register, security);
            return (user != null);
        }

        private User SyncLogin(ServerCommand cmd, Security security)
        {
            SM.User smUser;
            RequestResult result = Request(cmd, security, true);

            bool success = result.WaitForReturn<SM.User>(10000, out smUser);

            if (success)
            {
                return new User(smUser);
            }
            else
            {
                Connected = false;
                Writer.Join();
                Reader.Join();
                Server.GetStream().Dispose();
                Server.Close();
                return null;
            }
        }

        #region Write
        public RequestResult Request(ServerCommand cmd, object obj = null, bool immediate = false)
        {
            RequestResult ret = new RequestResult();
            int reqId = Interlocked.Increment(ref RequestId);

            lock (RequestedByServer)
            {
                RequestedByServer.Add(reqId, ret);
            }

            Payload payload = new Payload(CommType.Request, cmd, reqId, obj);

            if(immediate)
                QueueWrite(payload);

            return ret;
        }

        private void Return(int reqId, bool success, object obj)
        {
            Payload payload = new Payload(CommType.Return, reqId, success, obj);
            QueueWrite(payload);
        }

        public void Command(ServerCommand cmd, object obj = null)
        {
            Payload payload = new Payload(CommType.Void, cmd, obj);
            QueueWrite(payload);
        }

        private void QueueWrite(Payload payload)
        {
            lock (WriteQueue)
            {
                WriteQueue.Enqueue(payload);
                WriteQueueEmpty = false;
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
            ServerCommand cmd = (ServerCommand)payload.Command;
            int reqId = payload.RequestId;
            object obj = payload.Object;

            try
            {
                //Write CommType
                byte[] byteType = BitConverter.GetBytes((int)type);
                Server.GetStream().Write(byteType, 0, sizeof(int));

                //Write Command or 0/1 for return
                byte[] byteCmd = BitConverter.GetBytes((int)cmd);
                Server.GetStream().Write(byteCmd, 0, sizeof(int));

                //Write RequestId or 0 for void commands
                byte[] byteId = BitConverter.GetBytes(reqId);
                Server.GetStream().Write(byteId, 0, sizeof(int));

                //Write object length and object
                byte[] byteData = Serializer.Serialize(obj);
                byte[] length = BitConverter.GetBytes(byteData.Length);
                Server.GetStream().Write(length, 0, sizeof(int));
                Server.GetStream().Write(byteData, 0, byteData.Length);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }
        #endregion

        #region Read
        private void StartReader()
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            while (Connected)
            {
                CommType commType;
                ClientCommand cmd = 0;
                int reqId = -1;

                byte[] obj;
                int index;
                int bytesRead;
                int bytesToRead;

                //Get Comm Type
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType)BitConverter.ToInt32(buffer, 0);

                //Get Command or 0/1 for return
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ClientCommand)BitConverter.ToInt32(buffer, 0);

                //Get RequestId or 0 for void commands
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                reqId = BitConverter.ToInt32(buffer, 0);

                //Get Object Length
                Server.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                //Get object bytes
                obj = new byte[bytesToRead];
                index = 0;
                while (bytesToRead > 0)
                {
                    bytesRead = Server.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
                    Array.ConstrainedCopy(buffer, 0, obj, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }

                //Handle
                switch (commType)
                {
                    case CommType.Return:
                        HandleReturn(reqId, ((int)cmd == 1), obj);
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

        private void HandleVoid(ClientCommand cmd, byte[] obj)
        {
            throw new NotImplementedException();
        }

        private void HandleRequest(ClientCommand cmd, int reqId, byte[] objBytes)
        {
            throw new NotImplementedException();
        }

        private void HandleReturn(int reqId, bool success, byte[] objBytes)
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
                result.SetValue(success, obj);
            }
        }

        #endregion
    }
}
