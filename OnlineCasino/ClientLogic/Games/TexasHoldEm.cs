using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Games;
using SharedModels.Games.Events;
using ClientLogic.Players;

namespace ClientLogic.Games
{
    public class TexasHoldEm : Game
    {
        public List<CardPlayer> OtherPlayers
        {
            get
            {
                var players = Players.Where(p => p.Key != MainPlayer.Seat).Select(p => p.Value).ToList();
                return players.Cast<CardPlayer>().ToList();
            }
        }
        public TexasHoldEmPlayer MainPlayer;
        public TexasHoldEm(SharedModels.Games.Game game) : base(game)
        {
            foreach (var smPlayer in game.Players)
            {
                Players[smPlayer.Seat] = new TexasHoldEmPlayer(smPlayer);

                if (smPlayer.CurrentUser.UserID == ClientMain.MainUser.UserID)
                {
                    MainPlayer = (TexasHoldEmPlayer)Players[smPlayer.Seat];
                    ClientMain.MainPlayer = (Player)MainPlayer;
                }
            }
        }

        public override void HandleEvent(GameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        public override void Bet(int bet)
        {
            throw new NotImplementedException();
        }
    }
}
