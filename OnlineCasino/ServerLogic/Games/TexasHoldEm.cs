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
    public enum TexasHoldEmStates
    {
        Betting,
        Dealing,
        Playing,
        GainsOrLoses
    }

    public class TexasHoldEm : Game
    {
        public List<Card> tablecards;
                public TexasHoldEmStates TexasHoldEmState;

        public TexasHoldEm(List<User> users)
        {
            
            TexasHoldEmState = TexasHoldEmStates.Betting;

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

                Players.Add(new TexasHoldEmPlayer(u, buyIn));
            }
        }

        public void Start()
        {
            while (true)
            {
                
                bool noBet = false;

                //step1: wait for players.  Game state set to waiting
                //set_Game_State(GameStates.Waiting);

                /*SERVER CHECKS FOR CONNECTIONS 
                if(no connections) break;*/

                Deck deck = new Deck();

                TexasHoldEmState = TexasHoldEmStates.Betting;
                GameState = GameStates.Playing;

                foreach (TexasHoldEmPlayer player in Players)
                {
                    player.IndicateBet();
                }

                //BETTING
                decimal bet;

                foreach (TexasHoldEmPlayer player in Players)
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
                TexasHoldEmState = TexasHoldEmStates.Dealing;

                foreach (TexasHoldEmPlayer player in Players)
                {
                    Card card1 = deck.DealCard();
                    Card card2 = deck.DealCard();

                    player.DealCard(card1);
                    player.DealCard(card2);

                    Console.WriteLine("-------------------------------------");
                    Console.Out.Write("\n " + player.GetFullName() + " hand: \n");
                    CardHelper.PrintHand(player.GetCards());
                    Console.WriteLine("Count: " + CardHelper.CountHand(player.GetCards()));
                }

                Card tablecard1 = deck.DealCard();
                Card tablecard2 = deck.DealCard();
                Card tablecard3 = deck.DealCard();

                tablecards.Add(tablecard1);
                tablecards.Add(tablecard2);
                tablecards.Add(tablecard3);

           

                

                

                //PLAYING
               
                 if ( !noBet)
                {
                    TexasHoldEmState = TexasHoldEmStates.Playing;

                    foreach (TexasHoldEmPlayer player in Players)
                    {
                        Console.WriteLine("---------" + player.GetFullName() + "'s Turn --------");
                        bool done = false;

                        while (!done)
                        {
                            string decison;



                            try
                            {
                                Console.Out.Write("\n" + player.GetFullName() + "'s hand: \n");
                                CardHelper.PrintHand(player.GetCards());


                                Console.WriteLine("'Raise' or 'Check' or 'Fold' \n");
                                Reader();
                                decison = ReadLine(30000);
                            }
                            catch (TimeoutException)
                            {
                                Console.WriteLine("You waited too long... You will check.");
                                decison = "check";
                            }


                            switch (decison)
                            {
                                case "check":
                                    //bet what everyone else has
                                case "raise":
                                    //add more to the bet
                                case "fold":
                                    //quit
                                default:
                                    player.IndicateWait();
                                    done = true;
                                    break;
                            }
                        }
                    }
                }
                

           

               

                //GAME CONCLUSION
                TexasHoldEmState = TexasHoldEmStates.GainsOrLoses;
                foreach (TexasHoldEmPlayer player in Players)
                {
                    Console.Out.Write("\n-------" + player.GetFullName() + " Conclusion-------- \n");
                    int PlayersHand = CardHelper.CountHand(player.GetCards());

                    if (PlayersHand == 21)
                    {
                        Console.WriteLine("You got TexasHoldEm! +$" + player.UserBet);
                        player.UpdateGameBalance(true);
                    }
                    else if (PlayersHand > 21)
                    {
                        Console.WriteLine(player.GetFullName() + " busted. -$" + player.UserBet);
                        player.UpdateGameBalance(false);
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

                        if (Players.Count == 0)
                        {
                            Console.WriteLine("Press any key to exit the console..");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                }

                foreach (TexasHoldEmPlayer p in Players)
                {
                    if (!p.inGame) Players.Remove(p);
                }

                if (Players.Count == 0)
                {
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                //step8: loop back through game, check connections, if no connections break out of loop

                
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
