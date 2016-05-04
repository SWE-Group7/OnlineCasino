using ClientLogic.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;
using SMG = SharedModels.Games;
using System.Collections.Concurrent;
using ClientLogic.Players;
using SharedModels.Games.Events;
using SharedModels.Games.Enums;
using SharedModels.Connection.Enums;
using SMP = SharedModels.Players;
using SME = SharedModels.Games.Enums;

namespace ClientLogic.Games
{
    public class Blackjack : Game
    {
         
        public volatile SMG.BlackjackStates BlackjackState;
        public BlackjackHandStates HandState = BlackjackHandStates.Under21;
        public List<CardPlayer> OtherPlayers
        {
            get { var players = Players.Where(p => p.Key != MainPlayer.Seat).Select(p => p.Value).ToList();
                return players.Cast<CardPlayer>().ToList();
            }
        }
        public BlackjackPlayer MainPlayer;
        public List<Card> DealerHand;

        public Blackjack(SMG.Blackjack blackjack)
            :base(blackjack as SMG.Game)
        {
            
            BlackjackState = blackjack.BlackjackState;
            DealerHand = new List<Card>();

            foreach (var smPlayer in blackjack.Players)
            {
                Players[smPlayer.Seat] = new BlackjackPlayer(smPlayer);

                if (smPlayer.CurrentUser.UserID == ClientMain.MainUser.UserID)
                {
                    MainPlayer = (BlackjackPlayer)Players[smPlayer.Seat];
                    ClientMain.MainPlayer = (Player)MainPlayer;
                }
            }

        }


        public override void HandleEvent(GameEvent gEvent)
        {
            BlackjackEvent gameEvent = (BlackjackEvent)gEvent;
            BlackjackEvents bje = (BlackjackEvents)gameEvent.Event;

            switch (bje)
            {
                case BlackjackEvents.StartGame:
                    GS = GameStates.Playing;
                    break;
                case BlackjackEvents.EndGame:
                    
                    break;
                case BlackjackEvents.ChangeGameState:
                    ChangeState((SMG.BlackjackStates)gameEvent.Num);
                    break;
                case BlackjackEvents.PlayerJoin:
                    PlayerJoin(gameEvent.Player);
                    break;
                case BlackjackEvents.PlayerQuit:
                    PlayerQuit(gameEvent.Seat);
                    break;
                case BlackjackEvents.PlayerTurn:
                    Turn = gameEvent.Seat;
                    break;
                case BlackjackEvents.PlayerBet:
                    PlayerBet(gameEvent);
                    break;
                case BlackjackEvents.PlayerHit:
                    PlayerHit(gameEvent);
                    break;
                case BlackjackEvents.PlayerStay:
                    PlayerStay(gameEvent);
                    break;
                case BlackjackEvents.PlayerBust:
                    PlayerBust(gameEvent);
                    break;
                case BlackjackEvents.PlayerDouble:
                    break;
                case BlackjackEvents.ShowWinnings:
                    ShowWinnings(gameEvent.Seats, gameEvent.Winnings);
                    break;
                case BlackjackEvents.Deal:
                    Deal(gameEvent);
                    break;
                case BlackjackEvents.ShowDealer:
                    ShowDealer(gameEvent);
                    break;
            }
        }

        private void ChangeState( SMG.BlackjackStates state)
        {
            BlackjackState = state;

            if (BlackjackState == SMG.BlackjackStates.RoundFinish)
            {
                DealerHand.Clear();

                foreach(var pair in Players.ToList())
                {
                    var player = (BlackjackPlayer)pair.Value;
                    player.RefreshHand();
                    player.Bet = 0;
                    player.Gains = 0;

                }
                    
            }

            if(BlackjackState == SMG.BlackjackStates.Playing)
            {
                OS = OverallStates.Playing;
                GS = GameStates.Playing;
            }

                

           
        }

        private void ShowWinnings(byte[] seats, int[] winnings)
        {
            for (int i = 0; i < seats.Length; i++)
            {
                BlackjackPlayer player = ((BlackjackPlayer)Players[seats[i]]);
                player.Gains = winnings[i] - player.GameBalance;
                player.GameBalance = winnings[i];
            }
        }
        

        private void Deal(BlackjackEvent gameEvent)
        {
            DealerHand.Add(gameEvent.Cards[0][0]);
            for (int i = 1; i < gameEvent.Seats.Length; i++)
            {
                ((BlackjackPlayer)Players[gameEvent.Seats[i]]).UpdateHand(gameEvent.Cards[i][0]);
                ((BlackjackPlayer)Players[gameEvent.Seats[i]]).UpdateHand(gameEvent.Cards[i][1]);
            }   
        }
        private void ShowDealer(BlackjackEvent gameEvent)
        {
            DealerHand.Add(gameEvent.Cards[0][0]);   
        }
        private void PlayerBust(BlackjackEvent gameEvent)
        {
        }
        private void PlayerStay(BlackjackEvent gameEvent)
        {
        }

        private void PlayerHit(BlackjackEvent gameEvent)
        {
            if (gameEvent.Seat == 0)
                DealerHand.Add(gameEvent.Cards[0][0]);
            else
                ((BlackjackPlayer)Players[gameEvent.Seat]).UpdateHand(gameEvent.Cards[0][0]);
        }

        private void PlayerBet(BlackjackEvent gameEvent)
        {
            ((BlackjackPlayer)Players[gameEvent.Seat]).Bet = gameEvent.Num;
        }

        public void PlayerJoin(SMP.Player smPlayer)
        {
            Players[smPlayer.Seat] = new BlackjackPlayer(smPlayer);
        }

        public void PlayerQuit(int seat)
        {
            Player player;
            Players.TryRemove(seat, out player);
        }

    }

    public enum BlackjackHandStates
    {
        Under21,
        TwentyOne,
        Bust
    }
}
