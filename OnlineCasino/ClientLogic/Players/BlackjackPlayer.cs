using ClientLogic.Games;
using SharedModels.GameComponents;
using SharedModels.Games.Enums;
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
        public BlackjackHandStates HandState;
        public BlackjackEvents Action;

        public BlackjackPlayer(SMP.Player player)
            :base(player)
        {
            HandState = BlackjackHandStates.Under21;
            Action = (BlackjackEvents)0;
        }

        public override void RoundReset()
        {
            base.RoundReset();
            this.RefreshHand();

            HandState = BlackjackHandStates.Under21;
            Action = (BlackjackEvents)0;
        }

    }
}
