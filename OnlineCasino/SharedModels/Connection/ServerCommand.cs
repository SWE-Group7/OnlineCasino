using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Connection
{
    public enum ServerCommand
    {
        RequestCmd,
        ReturnCmd,
        Login,
        Register,
        Disconnect,
        GetUser,
    }

    public static class Extensions
    {
        
    }
}
