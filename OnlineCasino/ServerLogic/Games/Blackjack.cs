using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    class Blackjack : Game
    {
        private List<Card> DealerHand;
        public BlackjackStates BlackjackState;

        public Blackjack(List<User> users)
        {
            DealerHand = new List<Card>();
            BlackjackState = BlackjackStates.Betting;
                
            foreach (User u in users)
                {
                //Players.Add(new BlackjackPlayer(u, )
                }
            }

        public void Start()
        {
            while (true)
            {
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
               Console.Write("place your bet");
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
          
                }

            //Dealer gets cards and is checked for 21 
                Card dealercard1 = deck.DealCard();
                Card dealercard2 = deck.DealCard();
                DealerHand.Add(deck.DealCard());
                DealerHand.Add(deck.DealCard());

            //use Hayden's class to calculate numerical value
                int DealerAmount = CardHelper.CountHand(DealerHand);
        
            //step5: each player hits or stays  bjack state is now userplayig
                BlackjackState = BlackjackStates.Playing;

                foreach (BlackjackPlayer player in Players)
                {
                    //wait for player or 30sec
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    long i = stopwatch.ElapsedMilliseconds;

                    while (i < 30000)
                    {
                        Console.Out.Write("'hit' or 'stay'?\n");
                        String hitOrStay = Console.ReadLine();
                        switch (hitOrStay)
                        {
                            case "hit":
                                Card card = deck.DealCard();
                                player.DealCard(card);
                                stopwatch.Restart();
                                break;
                            default:
                                player.IndicateWait();
                                break;
                        }
                    } 
                }

                    //step6:dealer hits until 17 or over
                while (DealerAmount >= 17 && DealerAmount < 21)
                {
                    DealerHand.Add(deck.DealCard());
                    DealerAmount = CardHelper.CountHand(DealerHand);
                }

                    //setp7: Winnings are distributed  if over 21 no winnings
                foreach (BlackjackPlayer player in Players)
                {
                    int PlayersHand = CardHelper.CountHand(player.GetCards());

                    if (PlayersHand > DealerAmount)
                    {
                        BlackjackState = BlackjackStates.GainsOrLoses;
                        /*distribute winnings*/
                    }
                    else if (PlayersHand < DealerAmount)
                    { 
                        /*take bet amount*/
                    }
                    else
                    {
                        /*player takes only what he/she be. No more no less*/
                    }
                }

                //step8: loop back throu game, check connections, if no connections break out of loop

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
        
            while (i < 30000 && !continuePlay)
            {
                i = stopwatch.ElapsedMilliseconds;
                foreach (BlackjackPlayer player in Players)
                {
                    if (player.Status == BlackjackPlayerStatus.Betting)
                    {
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

