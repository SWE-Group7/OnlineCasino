using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Connection
{
    public enum ClientCommands
    {
        Disconnect,
        ForceQuit,
        SendEvent,

        Blackjack_GetAction,
        Blackjack_GetBet,
    }

}
