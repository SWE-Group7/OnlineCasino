using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Players;
using SharedModels.GameComponents;
using SMP = SharedModels.Players;

namespace ClientLogic.Players
{
    public abstract class Player
    {
        protected readonly User CurrentUser;
        public decimal GameBalance;
        public decimal Bet;
        public readonly int Seat;

        public Player(SMP.Player player)
        {
            this.CurrentUser = new User(player.CurrentUser);
            this.GameBalance = player.GameBalance;
            this.Seat = player.Seat;
        }

        public string getUsername()
        {
            return CurrentUser.Username;
        }

        public string getFullName()
        {
            return CurrentUser.FullName;
        }
    }
}
