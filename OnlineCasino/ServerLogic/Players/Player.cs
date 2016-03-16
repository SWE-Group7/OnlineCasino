using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public abstract class Player
    {
        User CurrentUser;
        float CurrentBalance;


        public Player()
        {
            //base class. Abstract
        }
    }
}
