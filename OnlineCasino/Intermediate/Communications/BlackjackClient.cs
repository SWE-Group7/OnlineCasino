using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediate.Communications
{
    class BlackjackClient : Client
    {
        public void PlaceBet(int bet)
        {
            //send bet to server
        }

        public void Hit(bool isHit)
        {
            //sends isHit to server to tell if hit or not
        }

        public void Stay(bool isStay)
        {
            //sends isStay to server to tell if stay or not
        }

    }
}
