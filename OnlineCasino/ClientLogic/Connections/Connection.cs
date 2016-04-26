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
using System.Diagnostics;

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
        private Dictionary<int, RequestResult> RequestedByClient;

        private Stopwatch Timer = new Stopwatch();

        public Connection()
        {
            Server = new TcpClient("192.168.0.16", 47689);
            Connected = true;
            WriteQueueEmpty = true;

            WriteQueue = new Queue<Payload>();
            RequestedByClient = new Dictionary<int, RequestResult>();

            Reader = Thread.CurrentThread;
            Writer = new Thread(() => StartWriter());
            Writer.Name = "Writer";
            Writer.Start();
            Reader = new Thread(() => StartReader());
            Reader.Name = "Reader";
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
            RequestResult result = Request(cmd, security);

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
        public RequestResult Request(ServerCommand cmd, object obj = null)
        {
            RequestResult ret = new RequestResult();
            int reqId = Interlocked.Increment(ref RequestId);

            lock (RequestedByClient)
            {
                RequestedByClient.Add(reqId, ret);
            }

            Payload payload = new Payload(CommType.Request, cmd, reqId, obj);
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
                if (!WriteQueueEmpty)
                {
                    Payload next;
                    lock (WriteQueue)
                    {
                        next = WriteQueue.Dequeue();
                        WriteQueueEmpty = !WriteQueue.Any();
                    }
                    ImmediateWrite(next);
                }
                Thread.Sleep(1);
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
                //Write Command Start
                byte[] byteStart = BitConverter.GetBytes((int)CommType.Start);
                Server.GetStream().Write(byteStart, 0, sizeof(int));

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
            CommType commType;
            ClientCommand cmd = 0;
            int reqId = -1;
            byte[] obj;
            int index;
            int bytesRead;
            int bytesToRead;

            while (Connected)
            {
                //If no data, continue
                if (!Server.GetStream().DataAvailable)
                {
                    Thread.Sleep(1);
                    continue;
                }

                //Ensure Command Start
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType)BitConverter.ToInt32(buffer, 0);
                if (commType != CommType.Start)
                    continue;

                //Ensure Reader isnt stuck
                if (!Server.GetStream().DataAvailable && ReaderStuck())
                    continue;

                //Get Comm Type
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommType)BitConverter.ToInt32(buffer, 0);

                if (!Server.GetStream().DataAvailable && ReaderStuck())
                    continue;

                //Get Command or 0/1 for return
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ClientCommand)BitConverter.ToInt32(buffer, 0);

                if (!Server.GetStream().DataAvailable && ReaderStuck())
                    continue;

                //Get RequestId or 0 for void commands
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                reqId = BitConverter.ToInt32(buffer, 0);

                if (!Server.GetStream().DataAvailable && ReaderStuck())
                    continue;

                //Get Object Length
                Server.GetStream().Read(buffer, 0, sizeof(int));
                bytesToRead = BitConverter.ToInt32(buffer, 0);

                //Get object bytes
                obj = new byte[bytesToRead];
                index = 0;
                while (bytesToRead > 0)
                {
                    if (!Server.GetStream().DataAvailable && ReaderStuck())
                        continue;

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

        private bool ReaderStuck()
        {
            Timer.Restart();
            while (Timer.ElapsedMilliseconds < ConnectionStatics.InbetweenReadTimeout)
            {
                Thread.Sleep(5);
                if (Server.GetStream().DataAvailable)
                    return false;
            }
            return true;
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
            lock (RequestedByClient)
            {
                if (RequestedByClient.TryGetValue(reqId, out result))
                    RequestedByClient.Remove(reqId);
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
