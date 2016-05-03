using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP = SharedModels.Players;

namespace ClientLogic.Players
{
    public class BlackjackPlayer : CardPlayer
    {
        public BlackjackPlayer(SMP.Player player)
            :base(player)
        {
        }
    }
}
