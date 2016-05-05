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
using System.Diagnostics;
using System.Collections.Concurrent;
using SharedModels.Connection.Enums;

namespace ServerLogic.Connections
{
    
    public class Connection
    {

        public bool Connected;
        public long LastRead;
        public long LastWrite;
        private int RequestId = 1;

        private Thread Reader;
        private Thread Writer;

        private Queue<Payload> WriteQueue;
        private volatile bool WriteQueueEmpty;

        private Queue<Payload> ReadQueue;
        private ConcurrentDictionary<int, RequestResult> RequestedByServer;

        private Stopwatch Timer = new Stopwatch();

        private TcpClient Client;
        private User CurrentUser;

        public Connection(TcpClient client)
        {
            this.Client = client;
            Connected = false;
            RequestedByServer = new ConcurrentDictionary<int, RequestResult>();
            WriteQueue = new Queue<Payload>();
            ReadQueue = new Queue<Payload>();
            WriteQueueEmpty = true;
        }

        public bool TryLogin(ServerCommands cmd, byte[] obj, out User userOut)
        {
            try
            {
                Security security = (Security)Serializer.Deserialize(obj);

                if (cmd == ServerCommands.Login)
                    CurrentUser = User.Login(security.Username, security.Password, this);
                else if (cmd == ServerCommands.Register)
                    CurrentUser = User.Register(security.Username, security.Password, security.EmailAddress, security.FullName, this);

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

            Connected = true;
            ImmediateWrite(new Payload(CommTypes.Return, reqId, true, smUser));
        }
        public void RejectLogin(int reqId, string errorMessage)
        {
            ImmediateWrite(new Payload(CommTypes.Return, reqId, false, errorMessage));
            Client.GetStream().Dispose();
            Client.Close();
            
        }

        public void Communicate()
        {
            string username = CurrentUser.Username;
            Reader = new Thread(() => StartReader());
            Reader.Name = String.Format("{0}_Reader", username);

            Writer = new Thread(() => StartWriter());
            Writer.Name = String.Format("{0}_Writer", username);
            Reader.Start();
            Writer.Start();
        }

        private void FinalDisconnect()
        {
            Connected = false;
            Reader.Join();
            Writer.Join();
            Client.GetStream().Close();
            Client.Close();

            CurrentUser.Connected = false;
        }

        #region Write

        public RequestResult Request(ClientCommands cmd, object obj = null)
        {
            RequestResult result = new RequestResult();
            int reqId = Interlocked.Increment(ref RequestId);

            RequestedByServer[reqId] = result;
            Payload payload = new Payload(CommTypes.Request, cmd, reqId, obj);
            QueueWrite(payload);

            return result;
        }

        public void Return(int reqId, bool success, object obj)
        {
            Payload payload = new Payload(CommTypes.Return, reqId, success, obj);
            QueueWrite(payload);   
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
            ClientCommands cmd = (ClientCommands) payload.Command;
            int reqId = payload.RequestId;
            object obj = payload.Object;

            try
            {
                //Write Command Start
                byte[] byteStart = BitConverter.GetBytes((int)CommTypes.Start);
                Client.GetStream().Write(byteStart, 0, sizeof(int));

                //Write CommTypes
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

        #endregion

        #region Read
        private void StartReader()
        {
            Stopwatch sw = new Stopwatch();
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            CommTypes commType;
            ServerCommands cmd = 0;
            int reqId = -1;

            byte[] objBytes;
            int index;
            int bytesRead;
            int bytesToRead;

            try
            {
                while (Connected)
                {

                    //If no data, continue
                    if (!Client.GetStream().DataAvailable)
                    {
                        Thread.Sleep(20);
                        continue;
                    }

                    //Ensure Command Start
                    bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                    commType = (CommTypes)BitConverter.ToInt32(buffer, 0);
                    if (commType != CommTypes.Start)
                        continue;

                    //Ensure Reader isnt stuck
                    if (!Client.GetStream().DataAvailable && ReaderStuck())
                        continue;

                    //Get Comm Type
                    bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                    commType = (CommTypes)BitConverter.ToInt32(buffer, 0);

                    if(!Client.GetStream().DataAvailable && ReaderStuck())
                        continue;

                    //Get Command or 0/1 for return
                    bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                    cmd = (ServerCommands)BitConverter.ToInt32(buffer, 0);

                    if (!Client.GetStream().DataAvailable && ReaderStuck())
                        continue;

                    //Get RequestId or 0 for void commands
                    bytesRead = Client.GetStream().Read(buffer, 0, sizeof(int));
                    reqId = BitConverter.ToInt32(buffer, 0);

                    if (!Client.GetStream().DataAvailable && ReaderStuck())
                        continue;

                    //Get Object Length
                    Client.GetStream().Read(buffer, 0, sizeof(int));
                    bytesToRead = BitConverter.ToInt32(buffer, 0);

                    //Get object bytes
                    objBytes = new byte[bytesToRead];
                    index = 0;
                    while (bytesToRead > 0)
                    {
                        if (!Client.GetStream().DataAvailable && ReaderStuck())
                            continue;

                        bytesRead = Client.GetStream().Read(buffer, 0, Math.Min(bufferSize, bytesToRead));
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

            catch (Exception ex)
            {
                ServerMain.WriteException("Connection.StartReader()", ex);
                Connected = false;
            }
        }

        private bool ReaderStuck()
        {
            Timer.Restart();
            while(Timer.ElapsedMilliseconds < ConnectionStatics.InbetweenReadTimeout)
            {
                Thread.Sleep(5);
                if (Client.GetStream().DataAvailable)
                    return false;
            }

            return true;
        }

        private void HandleVoid(ServerCommands cmd, object obj)
        {
            switch (cmd)
            {
                case ServerCommands.Disconnect:
                    this.FinalDisconnect();
                    break;
                default:
                    break;
            }
        }

        private void HandleRequest(ServerCommands cmd, int reqId, object obj)
        {
            object retObject = null;

            switch (cmd)
            {
                case ServerCommands.GetUserInfo:
                    if ((int)obj == CurrentUser.UserID)
                        retObject = CurrentUser.GetSharedModelPrivate();
                    else
                        retObject = ServerMain.GetUserInfo((int)obj);

                    Return(reqId, (retObject != null), retObject);
                    break;
                case ServerCommands.JoinGame:
                    int[] data = (int[])obj;
                    CurrentUser.JoinGame((GameTypes)data[0], data[1], reqId);
                    break;
            }
            
        }

        private void HandleReturn(int reqId, bool success, object obj)
        {
            RequestResult result;
            if (RequestedByServer.TryRemove(reqId, out result))
                result.SetValue(success, obj);
            
        }
        #endregion


    }
}
