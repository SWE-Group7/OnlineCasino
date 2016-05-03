using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Connection
{
    public enum ServerCommands
    {
        Login,
        Register,
        GetUserInfo,
        
        JoinGame,
        QuitGame,
        Disconnect
    }

}
