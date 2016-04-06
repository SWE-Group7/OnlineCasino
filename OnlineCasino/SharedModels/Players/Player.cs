using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    public abstract class Player
    {
        protected User CurrentUser;
        protected decimal GameBalance;
        decimal BuyIn;
    }
}
