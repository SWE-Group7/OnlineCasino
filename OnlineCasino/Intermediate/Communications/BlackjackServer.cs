using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;
using ServerLogic.Games;

namespace Intermediate.Communications
{
    class BlackjackServer : Server
    {
        public void DealCards(Card card, int seatNumber)
        {
            //Sends card from server to client with specific seat number
        }

        public void getState(BlackjackStates state)
        {
            //Send to client
        }
    }
}
