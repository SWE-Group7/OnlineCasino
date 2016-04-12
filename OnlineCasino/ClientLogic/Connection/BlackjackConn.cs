using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Connection
{
    public class BlackjackConn : BaseConnection
    {
        public BlackjackConn(string username, string password, string fullName, string email)
            :base(username, password, fullName, email)
        {

        }

        public void IndicateHit()
        {

        }

        public void IndicateStay()
        {

        }
    }
}
