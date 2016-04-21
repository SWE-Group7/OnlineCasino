using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharedModels.Connection
{
    public class Payload
    {
        public readonly CommType Type;
        public readonly int RequestId;
        public readonly int Command;
        public readonly object Object;

        private Payload(CommType type, int command, int requestId, object obj)
        {
            Type = type;
            Command = command;
            RequestId = requestId;
            Object = obj;
        }

        //Request
        public Payload(CommType type, ClientCommand cmd, int requestId, object obj)
            : this(type, (int)cmd, requestId, obj) { }

        public Payload(CommType type, ServerCommand cmd, int requestId, object obj)
            : this(type, (int)cmd, requestId, obj) { }


        //Void
        public Payload(CommType type, ClientCommand cmd, object obj)
            : this(type, (int)cmd, 0, obj) { }

        public Payload(CommType type, ServerCommand cmd, object obj)
            : this(type, (int)cmd, 0, obj) { }
        
        //Return
        public Payload(CommType type, int requestId, bool returnData, object obj)
            : this(type, (returnData) ? 1 : 0, requestId, obj) { }

    }
        
    
}
