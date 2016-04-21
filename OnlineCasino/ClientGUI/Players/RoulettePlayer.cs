using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.Players
{
    public class RoulettePlayer : Player
    {
        public RoulettePlayer(SharedModels.Players.RoulettePlayer R, int buyIn, int bet):
            base(R, buyIn, bet)
        {
        }
    }
}
