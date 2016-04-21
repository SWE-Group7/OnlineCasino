using SharedModels;
using SharedModels.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Connections.CommandHandlers
{
    public class BlackJackCH : ClientHandler
    {
        private RequestResult BlockingReturn;

        private bool waiting = false;
        public bool Waiting
        {
            get
            {
                return !BlockingReturn.HasValue();
            }
        }

        public BlackJackCH(Connection connection)
            : base(connection)
        {
        }

        public void IndicateBet()
        {
            waiting = true;
            ClientCommand cmd = ClientCommand.Blackjack_IndicateBet;
            BlockingReturn = connection.Request(cmd);
            
        }
        public bool GetBet(out decimal bet)
        {
            bet = 0;

            if (!Waiting)
            {
                bet = (decimal) BlockingReturn.GetValue();
                return true;
            } else
            {
                return false;
            }


        }

        public void IndicateHit()
        {
            
        }

        private void setReturn(ClientCommand cmd, RequestResult async)
        {
            if(AsyncReturns.ContainsKey(cmd))
                AsyncReturns[cmd] = 
        }
    }
}
