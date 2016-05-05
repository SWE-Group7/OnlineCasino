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
        public BlackjackPlayer TurnPlayer
        {
            get
            {
                if (Turn == 0)
                    return null;
                else
                    return (BlackjackPlayer)Players[Turn];
            }
        }
        public List<Card> DealerHand;
        public BlackjackEvents DealerAction;
        public BlackjackHandStates DealerHandState;

        public Blackjack(SMG.Blackjack blackjack)
            :base(blackjack as SMG.Game)
        {
            
            BlackjackState = blackjack.BlackjackState;
            DealerHand = new List<Card>();
            DealerHandState = BlackjackHandStates.Under21;
            DealerAction = (BlackjackEvents)0;

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
                    GS = SMG.GameStates.Playing;
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
                case BlackjackEvents.Payout:
                    Payout(gameEvent.Seats, gameEvent.WinStates, gameEvent.Winnings);
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

            switch (state)
            {
                case SMG.BlackjackStates.RoundStart:
                    GS = SMG.GameStates.Playing;
                    break;
                case SMG.BlackjackStates.Betting:
                    ClientMain.ClientState = ClientStates.Betting;
                    break;
                case SMG.BlackjackStates.Dealing:
                    ClientMain.ClientState = ClientStates.Game;
                    break;
                case SMG.BlackjackStates.RoundFinish:
                    DealerHand.Clear();
                    DealerHandState = BlackjackHandStates.Under21;
                    DealerAction = (BlackjackEvents)0;

                    foreach (var pair in Players.ToList())
                    {
                        var player = (BlackjackPlayer)pair.Value;
                        player.RoundReset();
                    }
                    break;
                case SMG.BlackjackStates.Playing:
                    GS = SMG.GameStates.Playing;
                    break;
                default:
                    break;

            }   
        }

        private void Payout(byte[] seats, byte[] winStates, int[] winnings)
        {
            for (int i = 0; i < seats.Length; i++)
            {
                BlackjackPlayer player = ((BlackjackPlayer)Players[seats[i]]);
                player.Gains = winnings[i] - player.GameBalance;
                player.GameBalance = winnings[i];
                player.WinLossState = (SMP.WinLossStates) winStates[i];
                   
            }
        }       

        private void Deal(BlackjackEvent gameEvent)
        {
            DealerHand.Add(gameEvent.Cards[0][0]);
            
            for (int i = 1; i < gameEvent.Seats.Length; i++)
            {
                ((BlackjackPlayer)Players[gameEvent.Seats[i]]).UpdateHand(gameEvent.Cards[i][0]);
                ((BlackjackPlayer)Players[gameEvent.Seats[i]]).UpdateHand(gameEvent.Cards[i][1]);

                if (CardHelper.CountHand(((BlackjackPlayer)Players[gameEvent.Seats[i]]).Hand) == 21)
                    HandState = BlackjackHandStates.TwentyOne;
            }   
        }
        private void ShowDealer(BlackjackEvent gameEvent)
        {
            DealerHand.Add(gameEvent.Cards[0][0]);
            if (CardHelper.CountHand(DealerHand) == 21)
                DealerHandState = BlackjackHandStates.TwentyOne;
        }
        private void PlayerBust(BlackjackEvent gameEvent)
        {
            if(gameEvent.Seat == 0)
            {
                DealerHandState = BlackjackHandStates.Bust;
            }
            else
            {
                BlackjackPlayer player = (BlackjackPlayer)Players[gameEvent.Seat];
                player.HandState = BlackjackHandStates.Bust;
            }
                
        }
        private void PlayerStay(BlackjackEvent gameEvent)
        {
            if (gameEvent.Seat == 0)
            {
                DealerAction = BlackjackEvents.PlayerStay;
                if (CardHelper.CountHand(DealerHand) == 21)
                    DealerHandState = BlackjackHandStates.TwentyOne;
            }
            else
            {
                BlackjackPlayer player = (BlackjackPlayer)Players[gameEvent.Seat];
                player.Action = BlackjackEvents.PlayerStay;

                if (CardHelper.CountHand(player.Hand) == 21)
                    DealerHandState = BlackjackHandStates.TwentyOne;
            }
        }

        private void PlayerHit(BlackjackEvent gameEvent)
        {
            if (gameEvent.Seat == 0)
            {
                DealerHand.Add(gameEvent.Cards[0][0]);
                DealerAction = BlackjackEvents.PlayerHit;

                if (CardHelper.CountHand(DealerHand) == 21)
                    DealerHandState = BlackjackHandStates.TwentyOne;
            }
            else
            {
                BlackjackPlayer player = (BlackjackPlayer)Players[gameEvent.Seat];

                player.UpdateHand(gameEvent.Cards[0][0]);

                //Update hand state if necessary
                if (CardHelper.CountHand(player.Hand) == 21)
                    player.HandState = BlackjackHandStates.TwentyOne;

                //Update action
                player.Action = BlackjackEvents.PlayerHit;

            }
                
        }

        private void PlayerBet(BlackjackEvent gameEvent)
        {
            BlackjackPlayer player = (BlackjackPlayer)Players[gameEvent.Seat];
            player.SetBet(gameEvent.Num);

            if (gameEvent.Seat == MainPlayer.Seat)
                ClientMain.ClientState = ClientStates.Game;

            player.Action = BlackjackEvents.PlayerBet;
        }

        public void PlayerJoin(SMP.Player smPlayer)
        {
            if(!Players.ContainsKey(smPlayer.Seat))
                Players[smPlayer.Seat] = new BlackjackPlayer(smPlayer);
        }

        public void PlayerQuit(int seat)
        {
            Player player;
            Players.TryRemove(seat, out player);
            ClientMain.MainUser.Balance += player.GameBalance - player.BuyIn;
            ClientMain.QuitGame();
        }

        public override void Bet(int bet)
        {
            ClientMain.HandleRequests(SharedModels.Connection.ClientCommands.Blackjack_GetBet, bet);
        }

        public void Action(BlackjackEvents action)
        {
            switch (action)
            {
                case BlackjackEvents.PlayerBet:
                case BlackjackEvents.PlayerStay:
                case BlackjackEvents.PlayerHit:
                    ClientMain.HandleRequests(SharedModels.Connection.ClientCommands.Blackjack_GetAction, action);
                    break;
                default:
                    break;
            }
        }
        
    }

    public enum BlackjackHandStates
    {
        Under21,
        TwentyOne,
        Bust
    }
}
