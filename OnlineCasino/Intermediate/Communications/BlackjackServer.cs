using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;
using ServerLogic;
using ServerLogic.Players;

namespace Intermediate.Communications
{
    class BlackjackServer : Server
    {
        public void DealCards(Card card, BlackjackPlayer player)
        {
            //Sends card from server to client with specific seat number
        }
    }
}
