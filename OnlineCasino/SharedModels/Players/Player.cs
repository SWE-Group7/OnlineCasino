using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    [Serializable]
    public abstract class Player
    {
        public readonly User CurrentUser;
        public readonly decimal GameBalance;

        public Player(User u, decimal gb)
        {
            CurrentUser = u;
            GameBalance = gb;
        }
    }
}
