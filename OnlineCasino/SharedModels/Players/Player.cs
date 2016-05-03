using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Players
{
    [Serializable]
    public class Player
    {
        public readonly User CurrentUser;
        public readonly byte Seat;
        public readonly int GameBalance;

        public Player(User user, int seat, int gameBalance)
        {
            CurrentUser = user;
            Seat = (byte) seat;
            GameBalance = gameBalance;
        }

    }
}
