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
                Console.WriteLine("----------------------------");
                Console.Out.Write(u.FullName + "'s Balance: $" + u.Balance + "\n");
                Console.Out.Write("Enter your buy in: $");

                string buyInStr = Console.ReadLine();
                decimal buyIn;

                while (!decimal.TryParse(buyInStr, out buyIn) || buyIn > u.Balance)
                {
                    Console.Out.Write("You can't afford that buy in. Try Again: \n");
                    Console.Out.Write("Enter your buy in: $");

                    buyInStr = Console.ReadLine();
                }

                Players.Add(new BlackjackPlayer(u, buyIn));
            }
        }

        public void Start()
        {
            while (true)
            {
                bool DealerWin = false;
                bool noBet = false;

                //step1: wait for players.  Game state set to waiting
                //set_Game_State(GameStates.Waiting);

                /*SERVER CHECKS FOR CONNECTIONS 
                if(no connections) break;*/

                Deck deck = new Deck();

                BlackjackState = BlackjackStates.Betting;
                GameState = GameStates.Playing;

                foreach (BlackjackPlayer player in Players)
                {
                    player.IndicateBet();
                }

                //BETTING
                decimal bet;
                
                foreach (BlackjackPlayer player in Players)
                {
                    Console.WriteLine("------" + player.GetFullName() + "'s Turn -----");
                    string output = " ";
                    do
                    {
                        try
                        {
                            Console.WriteLine("Place your bet: \n");
                            Reader();
                            output = ReadLine(30000);
                        }
                        catch (TimeoutException)
                        {
                            Console.WriteLine("You waited too long...Sit this one out.\n");
                            player.ForceNoBet();
                        }
                    }
                    while (!decimal.TryParse(output, out bet));

                    if (bet > player.getGameBalance())
                    {
                        bet = player.getGameBalance();
                        Console.Out.Write("Over your balance, bet set to: " + bet + "\n");
                    }

                    player.SetUserBet(bet);
                }
                Console.Clear();

                //DEALING
                BlackjackState = BlackjackStates.Dealing;
        
                foreach (BlackjackPlayer player in Players)
                {
                    Card card1 = deck.DealCard();
                    Card card2 = deck.DealCard();

                    player.DealCard(card1);
                    player.DealCard(card2);

                    Console.WriteLine("-------------------------------------");
                    Console.Out.Write("\n " + player.GetFullName() +" hand: \n");
                    CardHelper.PrintHand(player.GetCards());
                    Console.WriteLine("Count: " + CardHelper.CountHand(player.GetCards()));
                }

                Card dealercard1 = deck.DealCard();
                Card dealercard2 = deck.DealCard();

                DealerHand.Add(dealercard1);
                DealerHand.Add(dealercard2);

                Console.Out.Write("\n Dealer cards: \n HIDDEN of HIDDEN \n" + dealercard2.Rank + " of " + dealercard2.Suit + "\n");
                
                int DealerAmount = CardHelper.CountHand(DealerHand);

                //PLAYING
                if (DealerAmount == 21)
                {
                    DealerWin = true;
                    Console.Out.Write("Dealer has blackjack.\n GAME OVER\n");                   
                }
                else if (!DealerWin && !noBet)
                {
                    BlackjackState = BlackjackStates.Playing;

                    foreach (BlackjackPlayer player in Players)
                    {
                        Console.WriteLine("---------" + player.GetFullName() + "'s Turn --------");
                        bool stay = false;
                         
                        while (stay == false)
                        {
                            string hitOrStay;

                            if (CardHelper.CountHand(player.GetCards()) > 21)
                            {
                                hitOrStay = "stay";
                                Console.Out.Write("\n" + player.GetFullName() + "'s hand: \n");
                                CardHelper.PrintHand(player.GetCards());
                                Console.WriteLine("Count: " + CardHelper.CountHand(player.GetCards()));
                            }
                            else if (CardHelper.CountHand(player.GetCards()) == 21)
                            {
                                hitOrStay = "stay";

                                Console.WriteLine("BLACKJACK");
                                Console.Out.Write("\n" + player.GetFullName() + "'s hand: \n");
                                CardHelper.PrintHand(player.GetCards());
                                Console.WriteLine("Count: " + CardHelper.CountHand(player.GetCards()));
                            }
                            else
                            {
                                try
                                {
                                    Console.Out.Write("\n" + player.GetFullName() + "'s hand: \n");
                                    CardHelper.PrintHand(player.GetCards());
                                    Console.WriteLine("Count: " + CardHelper.CountHand(player.GetCards()));

                                    Console.WriteLine("'hit' or 'stay' \n");
                                    Reader();
                                    hitOrStay = ReadLine(30000);
                                }
                                catch (TimeoutException)
                                {
                                    Console.WriteLine("You waited too long... You will stay.");
                                    hitOrStay = "stay";
                                }
                            }                            

                            switch (hitOrStay)
                            {
                                case "hit":
                                    Card card = deck.DealCard();
                                    player.DealCard(card);
                                    break;
                                default:
                                    player.IndicateWait();
                                    stay = true;
                                    break;
                            }
                        }
                    }
                }

                //DEALER DRAW
                
                Console.WriteLine("-------------------------------------");
                Console.Out.Write("\nDealer's hand: \n");
                CardHelper.PrintHand(DealerHand);            
                 
                while (DealerAmount < 17)
                {
                    Card drawn = deck.DealCard();
                    DealerHand.Add(drawn);

                    Console.Out.Write("\nDealer draws: " + drawn.Rank + " of " + drawn.Suit + "\n");
                    DealerAmount = CardHelper.CountHand(DealerHand);
                }

                Console.WriteLine("Dealer's count: " + DealerAmount);

                //GAME CONCLUSION
                BlackjackState = BlackjackStates.GainsOrLoses;
                foreach (BlackjackPlayer player in Players)
                {
                    Console.Out.Write("\n-------" + player.GetFullName() + " Conclusion-------- \n");
                    int PlayersHand = CardHelper.CountHand(player.GetCards());

                    if(PlayersHand == 21)
                    {
                        Console.WriteLine("You got blackjack! +$" + player.Bet);
                        player.UpdateGameBalance(true);
                    }
                    else if (PlayersHand > 21)
                    {                       
                        Console.WriteLine(player.GetFullName() + " busted. -$" + player.Bet);
                        player.UpdateGameBalance(false);
                    }
                    else if (DealerAmount > 21)
                    {
                        Console.WriteLine("Dealer busted. You win! +$" + player.Bet);
                        player.UpdateGameBalance(true);
                    }
                    else if (PlayersHand < 21 && DealerAmount < 21 && PlayersHand < DealerAmount)
                    {
                        Console.WriteLine("You lose! -$" + player.Bet);
                        player.UpdateGameBalance(false);
                    }
                    else if (PlayersHand < 21 && DealerAmount < 21 && PlayersHand > DealerAmount)
                    {
                        Console.WriteLine("You win! +$" + player.Bet);
                        player.UpdateGameBalance(true);
                    }
                    else if(PlayersHand == DealerAmount)
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.WriteLine("You tied.\n");
                    }

                    Console.WriteLine(player.GetFullName() + "'s game balance: $" + player.getGameBalance());

                    player.ClearCards();
                    player.UpdateUserBalance();
                  
                    if (player.GetBalance() <= 0)
                    {
                        Console.WriteLine(player.GetFullName() + "'s total balance is now 0. Adding $10..");
                        player.GiftMoney(10);
                    }

                    if (player.getGameBalance() <= 0)
                    {
                        Console.WriteLine("\nYour game balance is 0! You lose the game!");
                        player.inGame = false;
                        
                        if(Players.Count == 0)
                        {
                            Console.WriteLine("Press any key to exit the console..");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                }

                foreach(BlackjackPlayer p in Players)
                {
                    if (!p.inGame) Players.Remove(p);
                }

                if(Players.Count == 0)
                {
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                //step8: loop back through game, check connections, if no connections break out of loop

                DealerHand = new List<Card>();
            }
                       
        }

        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static string input;

        static void Reader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);

            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }
         
        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }
        
        public static string ReadLine(int timeOutMillisecs)
        {
            getInput.Set();

            bool success = gotInput.WaitOne(timeOutMillisecs);

            if (success) return input;
            else throw new TimeoutException("User did not provide input within the time limit.");
        }
    }
 }
