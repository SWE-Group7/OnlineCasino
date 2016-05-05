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
using SharedModels.Connection.Enums;
using SharedModels.Games;
using SharedModels.Games.Events;

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
        private List<int> RequestedByServer;

        private Stopwatch Timer = new Stopwatch();

        public Connection()
        {
            Server = new TcpClient(ConnectionStatics.IP, ConnectionStatics.Port);
            Connected = true;
            WriteQueueEmpty = true;

            WriteQueue = new Queue<Payload>();
            RequestedByClient = new Dictionary<int, RequestResult>();
            RequestedByServer = new List<int>();

            Reader = Thread.CurrentThread;
            Reader = new Thread(() => StartReader());
            Reader.Name = "Reader";
            Reader.Start();

            Writer = new Thread(() => StartWriter());
            Writer.Name = "Writer";
            Writer.Start();

        }

        public bool TrySyncLogin(Security security, out User user)
        {
            user = SyncLogin(ServerCommands.Login, security);
            return (user != null);
        }

        public bool TrySyncRegister(Security security, out User user)
        {
            user = SyncLogin(ServerCommands.Register, security);
            return (user != null);
        }

        private User SyncLogin(ServerCommands cmd, Security security)
        {
            object smUser;
            RequestResult result = Request(cmd, security);

            bool success = result.WaitForReturn(10000, out smUser);

            if (success)
            {
                return new User((SM.User)smUser);
            }
            else
            {
                if (smUser is string)
                    ClientMain.SetLoginMessage((string)smUser);
                Connected = false;
                Writer.Join();
                Reader.Join();
                Server.GetStream().Dispose();
                Server.Close();
                return null;
            }
        }

        #region Write
        public RequestResult Request(ServerCommands cmd, object obj = null)
        {
            RequestResult ret = new RequestResult();
            int reqId = Interlocked.Increment(ref RequestId);

            lock (RequestedByClient)
            {
                RequestedByClient.Add(reqId, ret);
            }

            Payload payload = new Payload(CommTypes.Request, cmd, reqId, obj);
            QueueWrite(payload);

            return ret;
        }

        public void Return(int reqId, bool success, object obj)
        {
            if (RequestedByServer.Contains(reqId))
            {
                Payload payload = new Payload(CommTypes.Return, reqId, success, obj);
                QueueWrite(payload);
            }
        }

        public void Command(ClientCommands cmd, object obj = null)
        {
            Payload payload = new Payload(CommTypes.Void, cmd, obj);
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
            CommTypes type = payload.Type;
            ServerCommands cmd = (ServerCommands)payload.Command;
            int reqId = payload.RequestId;
            object obj = payload.Object;

            try
            {
                //Write Command Start
                byte[] byteStart = BitConverter.GetBytes((int)CommTypes.Start);
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
            CommTypes commType;
            ClientCommands cmd = 0;
            int reqId = -1;
            byte[] objBytes;
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
                commType = (CommTypes)BitConverter.ToInt32(buffer, 0);
                if (commType != CommTypes.Start)
                    continue;

                //Ensure Reader isnt stuck
                if (!Server.GetStream().DataAvailable && ReaderStuck())
                    continue;

                //Get Comm Type
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                commType = (CommTypes)BitConverter.ToInt32(buffer, 0);

                if (!Server.GetStream().DataAvailable && ReaderStuck())
                    continue;

                //Get Command or 0/1 for return
                bytesRead = Server.GetStream().Read(buffer, 0, sizeof(int));
                cmd = (ClientCommands)BitConverter.ToInt32(buffer, 0);

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
                objBytes = new byte[bytesToRead];
                index = 0;
                while (bytesToRead > 0)
                {
                    if (!Server.GetStream().DataAvailable && ReaderStuck())
                        continue;

                    bytesRead = Server.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
                    Array.ConstrainedCopy(buffer, 0, objBytes, index, bytesRead);
                    index += bytesRead;
                    bytesToRead -= bytesRead;
                }

                object obj = Serializer.Deserialize(objBytes);
                //Handle
                switch (commType)
                {
                    case CommTypes.Return:
                        HandleReturn(reqId, ((int)cmd == 1), obj);
                        break;
                    case CommTypes.Request:
                        HandleRequest(cmd, reqId, obj);
                        break;
                    case CommTypes.Void:
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

        private void HandleVoid(ClientCommands cmd, object obj)
        {
            switch (cmd)
            {
                case ClientCommands.SendEvent:
                    ClientMain.QueueEvent((GameEvent)obj);
                    break;
            }
        }

        private void HandleRequest(ClientCommands cmd, int reqId, object obj)
        {

            RequestResult request = new RequestResult();
            lock (RequestedByServer)
                RequestedByServer.Add(reqId);

            ClientMain.QueueRequest(cmd, reqId);


        }

        private void HandleReturn(int reqId, bool success, object obj)
        {
            RequestResult result;
            lock (RequestedByClient)
            {
                if (RequestedByClient.TryGetValue(reqId, out result))
                    RequestedByClient.Remove(reqId);
            }

            if(result != null)
                result.SetValue(success, obj);
            
        }

        #endregion
    }
}
