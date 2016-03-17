using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ServerLogic.Games.GameComponents;
using SharedModels.GameComponents;
using System.Diagnostics;

namespace ServerLogic.Games
{
    public enum BlackjackStates
    {
        Betting,
        Dealing,
        Playing,
        GainsOrLoses
    }

    public class Blackjack : Game
    {
        private List<Card> DealerHand;
        public BlackjackStates BlackjackState;

        public Blackjack(List<User> users)
        {
            DealerHand = new List<Card>();
            BlackjackState = BlackjackStates.Betting;

            foreach (User u in users)
            {
                Console.Out.Write("What is your buy in?");
                string buyInStr = Console.ReadLine();
                float buyIn = float.Parse(buyInStr);
                Players.Add(new BlackjackPlayer(u, buyIn));
            }
        }

        public void Start()
        {
            while (true)
            {

                Boolean DealerWin = false;
                Boolean won = false;
                //step1: wait for players.  Game state set to waiting
                //set_Game_State(GameStates.Waiting);

                /*SERVER CHECKS FOR CONNECTIONS 
                  if(no connections) break;*/

                //step2: Get deck...get from Hayden's Deck class!!
                Deck deck = new Deck();

                //step3: Let players bet. Set Bjack state to betting and game state to playing
                BlackjackState = BlackjackStates.Betting;
                GameState = GameStates.Playing;

                foreach (BlackjackPlayer player in Players)
                {
                    player.IndicateBet();
                }

                WaitForBets(); //wait for all bets or 30sec

                //step4:deal cards blackjack state changes to dealing.  CHECK DEALER HAND AND IF 21 GAMEOVER EVERYONE LOST
                BlackjackState = BlackjackStates.Dealing;

                foreach (BlackjackPlayer player in Players)
                {
                    Card card1 = deck.DealCard();
                    Card card2 = deck.DealCard();
                    player.DealCard(card1);
                    player.DealCard(card2);
                    Console.Out.Write("\n Your hand: \n");
                    CardHelper.PrintHand(player.GetCards());
                }

                //Dealer gets cards and is checked for 21 
                Card dealercard1 = deck.DealCard();
                Card dealercard2 = deck.DealCard();
                DealerHand.Add(dealercard1);
                DealerHand.Add(dealercard2);
                Console.Out.Write("\n Dealer cards: \n HIDDEN of HIDDEN \n" + dealercard2.Rank + " of " + dealercard2.Suit + "\n");

                //use Hayden's class to calculate numerical value
                int DealerAmount = CardHelper.CountHand(DealerHand);
                if (DealerAmount == 21)
                {
                    DealerWin = true;
                    Console.Out.Write("Dealer has blackjack.\n GAME OVER\n");
                }

                //step5: each player hits or stays  bjack state is now userplayig
                BlackjackState = BlackjackStates.Playing;

                if (!DealerWin)
                {
                    foreach (BlackjackPlayer player in Players)
                    {
                        //wait for player or 30sec
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        long i = stopwatch.ElapsedMilliseconds;
                        bool stay = false;

                        while ((i < 30000) && (stay == false))
                        {
                            i = stopwatch.ElapsedMilliseconds;

                            string hitOrStay;
                            if (CardHelper.CountHand(player.GetCards()) > 21)
                                hitOrStay = "stay";
                            else {
                                Console.Out.Write("'hit' or 'stay'?\n");
                                hitOrStay = Console.ReadLine();
                            }

                            switch (hitOrStay)
                            {
                                case "hit":
                                    Card card = deck.DealCard();
                                    player.DealCard(card);
                                    Console.Out.Write("\n Your hand: \n");
                                    CardHelper.PrintHand(player.GetCards());
                                    stopwatch.Restart();
                                    break;
                                default:
                                    player.IndicateWait();
                                    stay = true;
                                    break;
                            }
                        }
                    }
                }

                //step6:dealer hits until 17 or over

                Console.Out.Write("\nDealer's full hand: \n");
                CardHelper.PrintHand(DealerHand);

                while (DealerAmount < 17)
                {
                    Card drawn = deck.DealCard();
                    DealerHand.Add(drawn);
                    Console.Out.Write("\nDealer draws: " + drawn.Rank + " of " + drawn.Suit + "\n");
                    DealerAmount = CardHelper.CountHand(DealerHand);
                }

                //setp7: Winnings are distributed  if over 21 no winnings
                foreach (BlackjackPlayer player in Players)
                {
                    int PlayersHand = CardHelper.CountHand(player.GetCards());


                    if (PlayersHand > 21)
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.Out.Write("You Busted :( \n");
                        won = false;
                        player.UpdateGameBalance(won);
                    }
                    else if (DealerAmount > 21)
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.Out.Write("Dealer Busts!\n");
                        won = true;
                        player.UpdateGameBalance(won);
                    }
                    else if (PlayersHand < DealerAmount)
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.Out.Write("You lost :( \n");
                        won = false;
                        player.UpdateGameBalance(won);
                    }
                    else if (PlayersHand > DealerAmount)
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.Out.Write("You won!\n");
                        won = true;
                        player.UpdateGameBalance(won);
                    }
                    else
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.Out.Write("You tied....\n");
                    }
                }

                //step8: loop back through game, check connections, if no connections break out of loop
                foreach (BlackjackPlayer player in Players)
                {
                    player.ClearCards();
                    Console.Out.Write("\nYour game balance is: " + player.getGameBalance() + "\n");
                }

                DealerHand = new List<Card>();
            }

            //step9: if no connections, results saved to server
        }


        public void WaitForBets()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool continuePlay = false;

            long i = stopwatch.ElapsedMilliseconds;
            int flag = 0;

            Console.Out.Write("Place Bet: ");
            string bet;

            while (i < 30000)
            {
                i = stopwatch.ElapsedMilliseconds;
                foreach (BlackjackPlayer player in Players)
                {
                    if (player.Status == BlackjackPlayerStatus.Betting)
                    {
                        bet = Console.ReadLine();
                        if (bet != null)
                        {
                            player.SetUserBet(float.Parse(bet));
                            player.IndicateWait();
                        }
                    }
                    else
                    {
                        flag++;
                    }
                }
                if (flag == Players.Count)
                {
                    continuePlay = true;
                }
            }

            stopwatch.Stop();

            foreach (BlackjackPlayer player in Players)
            {
                if (player.Status == BlackjackPlayerStatus.Betting)
                {
                    player.ForceNoBet();
                }
            }
        }
    }
}