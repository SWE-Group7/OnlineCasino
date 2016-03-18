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
                Boolean noBet = false;
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
            
                //Betting 
                foreach (BlackjackPlayer player in Players)
                {
                    string output = " ";
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

                    float bet = float.Parse(output);
                    if (bet > player.getGameBalance())
                    {
                        bet = player.getGameBalance();
                        Console.Out.Write("Over your balance, bet set to: " + bet + "\n");
                    }
                    player.SetUserBet(bet);
                }

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

                if (!DealerWin && !noBet)
                {
                    foreach (BlackjackPlayer player in Players)
                    {
                        bool stay = false;

                        while (stay == false)
                        {
                            string hitOrStay;

                            if (CardHelper.CountHand(player.GetCards()) > 21)
                                hitOrStay = "stay";
                            else {
                                try
                                {
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
                                    Console.Out.Write("\n Your hand: \n");
                                    CardHelper.PrintHand(player.GetCards());
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

                while(DealerAmount < 17)
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
                        /*take bet amount*/
                    }
                    else
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        Console.Out.Write("You tied....\n");
                    }
                }

                //step8: loop back throu game, check connections, if no connections break out of loop

                DealerHand = new List<Card>();

                foreach(BlackjackPlayer player in Players)
                {
                    if (player.getGameBalance() <= 0)
                    {
                        Console.Out.Write("You are broke\nGoodbye.");
                        Environment.Exit(0);
                    }
                }
            }

            //step9: if no connections, results saved to server
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
            if (success)
                return input;
            else
                throw new TimeoutException("User did not provide input within the time limit.");
        }
    }
}
