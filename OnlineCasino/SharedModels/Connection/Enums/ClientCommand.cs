using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Connection
{
    public enum ClientCommand
    {
        Disconnect,

        //Return Success
        ReturnSuccess,
        ReturnFailure,
        

        //Games
        SendCard,
        
        //Blackjack
        Blackjack_IndicateBet,
        Blackjack_SendPlayer,

    }
}
