using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Connection
{
    public class Payload
    {
        public readonly int Command;
        public readonly ulong ID;
        public readonly byte[] Object;

        public Payload(int command, ulong id, byte[] obj)
        {
            Command = command;
            ID = id;
            Object = obj;
        }

        public Payload(ServerCommand command, ulong id, byte[] obj)
        {
            Command = (int) command;
            ID = id;
            Object = obj;
        }

        public Payload(ClientCommand command, ulong id, byte[] obj)
        {
            Command = (int) command;
            ID = id;
            Object = obj;
        }
    }
}
