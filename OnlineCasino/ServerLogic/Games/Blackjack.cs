
using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using ServerLogic.Games.GameComponents;
using SharedModels.GameComponents;
using System.Diagnostics;
using SharedModels.Connection.Enums;
using SM = SharedModels;
using SMP = SharedModels.Players;
using SharedModels.Games;
using SMG = SharedModels.Games;
using SharedModels.Connection;
using SharedModels.Games.Enums;
using SharedModels.Games.Events;

namespace ServerLogic.Games
{
    public class Blackjack : Game
    {

        protected override int MaxSeats { get; } = 5;
        private BlackjackStates BlackjackState;
        private Deck deck;
        private List<Card> DealerHand;
        private const int TimeLimit = 15000;
        private List<int> Naturals;

        public Blackjack(int Id)
            :base(Id)
        {
            DealerHand = new List<Card>();
            BlackjackState = BlackjackStates.Betting;
            Naturals = new List<int>();
        }
        
        protected override void Run()
        {

            //Wait for player
            bool playersExist = false;
            while (!playersExist)
            {
                playersExist = Players.AsEnumerable().Where(p => p.Value.GetState() != SMP.PlayerStates.Joining).Any();
                Thread.Sleep(100);
            }

            sw.Restart();
            while (sw.ElapsedMilliseconds < 1000)
                Thread.Sleep(50);
            

            GameState = GameStates.Playing;
            GameEvent gameEvent = BlackjackEvent.StartGame();
            Broadcast(gameEvent);


            while (!EndGame)
            {
                //Round Start
                ResetRoundVariables();
                BlackjackState = BlackjackStates.RoundStart;
                deck = new Deck();
                ClientCommands cmd;
                gameEvent = BlackjackEvent.ChangeState(BlackjackState);
                Broadcast(gameEvent);

                //Start Betting
                BlackjackState = BlackjackStates.Betting;
                gameEvent = BlackjackEvent.ChangeState(BlackjackState);
                Broadcast(gameEvent);

                //Request Bets
                cmd = ClientCommands.Blackjack_GetBet;
                foreach (BlackjackPlayer player in ActivePlayers)
                {
                    player.Request(cmd);
                }

                //Recieve Bets
                List<Player> NoResponse = ActivePlayers;
                sw.Restart();
                while (sw.ElapsedMilliseconds < TimeLimit)
                {
                    foreach (BlackjackPlayer player in NoResponse)
                    {
                        int bet;

                        if (player.TryGetResult(cmd, out bet))
                        {
                            player.SetBet(bet);
                            gameEvent = BlackjackEvent.PlayerBet(player.Seat, bet);
                            Broadcast(gameEvent);
                            NoResponse.RemoveAll(p => p.Seat == player.Seat);

                            if (!NoResponse.Any())
                                break;

                        }

                        Thread.Sleep(10);
                    }

                    if (!NoResponse.Any())
                        break;
                }

                //Force NoResponses to bet 0
                foreach (BlackjackPlayer player in NoResponse)
                {
                    int bet = 0;
                    player.SetBet(bet);
                    gameEvent = BlackjackEvent.PlayerBet(player.Seat, bet);
                    Broadcast(gameEvent);
                }


                //DEALING
                BlackjackState = BlackjackStates.Dealing;
                gameEvent = BlackjackEvent.ChangeState(BlackjackState);
                Broadcast(gameEvent);

                DealerHand.Clear();
                DealerHand.Add(deck.DealCard());
                DealerHand.Add(deck.DealCard());

                Dictionary<int, Card[]> seatToCards = new Dictionary<int, Card[]>();
                seatToCards.Add(0, new Card[] { DealerHand[0] });

               
                foreach (BlackjackPlayer player in ActivePlayers)
                {
                    Card card1 = deck.DealCard();
                    Card card2 = deck.DealCard();
                    player.ClearCards();
                    player.DealCard(card1);
                    player.DealCard(card2);
                    seatToCards.Add(player.Seat, new Card[] { card1, card2 });
                    if (player.CardCount == 21)
                        Naturals.Add(player.Seat);
                }

                

                gameEvent = BlackjackEvent.Deal(seatToCards);
                Broadcast(gameEvent);

                Thread.Sleep(2000);
                //PLAYING
                BlackjackState = BlackjackStates.Playing;
                gameEvent = BlackjackEvent.ChangeState(BlackjackState);

                int DealerCount = CardHelper.CountHand(DealerHand);
                bool DealerBlackjack = (DealerCount == 21);
                if (!DealerBlackjack)
                {
                    cmd = ClientCommands.Blackjack_GetAction;

                    foreach (BlackjackPlayer player in ActivePlayers)
                    {
                        //Show Turn
                        gameEvent = BlackjackEvent.PlayerTurn(player.Seat);
                        Broadcast(gameEvent);

                        bool StayOrBust = false;
                        do
                        {
                            BlackjackEvents action = (BlackjackEvents)0;

                            //Get Action
                            player.Request(cmd);
                            sw.Restart();
                            while(sw.ElapsedMilliseconds < player.TimeLimit)
                            {
                                if (player.TryGetResult(cmd, out action))
                                    break;

                                Thread.Sleep(5);
                            }

                            switch (action)
                            {
                                case BlackjackEvents.PlayerHit:
                                    Card card = deck.DealCard();
                                    player.DealCard(card);
                                    gameEvent = BlackjackEvent.PlayerHit(player.Seat, card);
                                    break;
                                case BlackjackEvents.PlayerDouble:
                                case BlackjackEvents.PlayerStay:
                                default:
                                    gameEvent = BlackjackEvent.PlayerStay(player.Seat);
                                    StayOrBust = true;
                                    break;
                            }
                            Broadcast(gameEvent);

                            if (player.CardCount == 21)
                                StayOrBust = true;
                            else if (player.CardCount > 21)
                            {
                                StayOrBust = true;
                                gameEvent = BlackjackEvent.PlayerBust(player.Seat);
                                Broadcast(gameEvent);
                            }
                        } while (!StayOrBust);

                    }
                }

                //DEALER TURN
                gameEvent = BlackjackEvent.PlayerTurn(0);
                Broadcast(gameEvent);
                gameEvent = BlackjackEvent.ShowDealer(DealerHand[1]);
                Broadcast(gameEvent);
                
                while (DealerCount < 17)
                {
                    Card card = deck.DealCard();
                    DealerHand.Add(card);
                    DealerCount = CardHelper.CountHand(DealerHand);
                    gameEvent = BlackjackEvent.PlayerHit(0, card);
                    Broadcast(gameEvent);
                }

                if (DealerCount <= 21)
                    gameEvent = BlackjackEvent.PlayerStay(0);
                else
                    gameEvent = BlackjackEvent.PlayerBust(0);

                Broadcast(gameEvent);

                //GAME CONCLUSION
                BlackjackState = BlackjackStates.Payout;
                gameEvent = BlackjackEvent.ChangeState(BlackjackState);
                Broadcast(gameEvent);

                Dictionary<int, int> seatToWinnings = new Dictionary<int, int>();
                foreach (BlackjackPlayer player in ActivePlayers)
                {
                    int PlayerCount = player.CardCount;
                    bool playerBlackjack = Naturals.Contains(player.Seat);
                    decimal mult = 0;

                    if (PlayerCount > 21)
                        mult = -1;
                    else if (DealerCount <= 21 && PlayerCount < DealerCount)
                        mult = -1;
                    else if (DealerBlackjack && !playerBlackjack)
                        mult = -1;
                    else if (DealerCount == PlayerCount && DealerCount != 21)
                        mult = 0;
                    else if (DealerBlackjack && playerBlackjack)
                        mult = 0;
                    else if (!DealerBlackjack && playerBlackjack)
                        mult = 1.5m;
                    else
                        mult = 1m;

                    int balance = player.UpdateGameBalance(mult);
                    seatToWinnings[player.Seat] = balance;
                }

                gameEvent = BlackjackEvent.Payout(seatToWinnings);
                Broadcast(gameEvent);

                gameEvent = BlackjackEvent.ChangeState(BlackjackStates.RoundFinish);
                Broadcast(gameEvent);

                PurgeQuitters();
            }
                       
        }

        public override Player TakeSeat(User user, int buyIn)
        {
            int seat = TakeNextSeat();

            //No open seats
            if (seat < 0)
                return null;

            BlackjackPlayer player = new BlackjackPlayer(user, this, seat, buyIn);

            lock (Players)
                Players.TryAdd(seat, player);

            RoundSnapshot = GetSharedModel();
            BlackjackEvent gameEvent = BlackjackEvent.PlayerJoin(player.GetSharedModel());
            Broadcast(gameEvent);
            return player;
        }
        protected override SM.Games.Game GetSharedModel()
        {
            SMG.Game game;
            List<Player> players;
            List<SMP.Player> smPlayers = new List<SMP.Player>();

            players = Players.AsEnumerable()
                             .Select(p => p.Value)
                             .ToList();

            foreach(Player p in players)
                smPlayers.Add(p.GetSharedModel());

            game = new SMG.Blackjack(smPlayers, GameState, BlackjackState);
            return game;

        }
    }
 }
