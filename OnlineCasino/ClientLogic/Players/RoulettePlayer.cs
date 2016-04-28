using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP = SharedModels.Players;

namespace ClientLogic.Players
{
    public class RoulettePlayer : Player
    {
        public RoulettePlayer(SMP.Player player)
            :base(player)
        {
        }
    }
}
