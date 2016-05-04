using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Games.Enums
{

    public enum BlackjackEvents
    {
        StartGame,
        EndGame,
        ChangeGameState,
        PlayerJoin,
        PlayerQuit,

        PlayerBet,
        PlayerHit,
        PlayerStay,
        PlayerDouble,
        PlayerBust,
        PlayerTurn,

        Deal,
        ShowDealer,
        Payout,
    }  

    
}
