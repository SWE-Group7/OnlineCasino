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
    public enum BlackjackStates {
        //flags for states
       Betting, Dealing,UserPlaying,GainsorLoses}

    class Blackjack : Game
    {

        private List<Card> Dealer;//keeps track of dealer hand
         
        //enum variables to define states within the game
        BlackjackStates Bjackstate;
       GameStates gamestate;
        BlackjackPlayerStatus Playerstatus;


        private List<BlackjackPlayer> blackjackPlayers = null;
        public List<BlackjackPlayer> BlackjackPlayers

        {
            get
            {
                if(blackjackPlayers == null)
                    blackjackPlayers = Players.OfType<BlackjackPlayer>().ToList();
                
                return blackjackPlayers;               
            }
            
        }
        public void Timer() {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Boolean continueplay = false;
            long i = stopwatch.ElapsedMilliseconds;
            int flag = 0; ;
            while (i < 30000 && continueplay==false) {
                i = stopwatch.ElapsedMilliseconds;
                foreach (BlackjackPlayer player in blackjackPlayers)
                {
                    if (Playerstatus == BlackjackPlayerStatus.Betting) { }
                    else { flag++; }
                    

                }
                if (flag == blackjackPlayers.Count) { continueplay = true; }
            }
            stopwatch.Stop();
            foreach (BlackjackPlayer player in blackjackPlayers)
            {
                if (Playerstatus == BlackjackPlayerStatus.Betting) { player.ForceNoBet();}

            }
        }
        private static System.Timers.Timer aTimer;

      

        //methods that can be called to set states and get states. Get states will be used to verify if ok for a new player to join
        public void set_Game_State(GameStates state) { state = gamestate;  }

        public void set_Bjack_State(BlackjackStates bjackstate) { bjackstate = Bjackstate; }

        public BlackjackStates get_Bjack_State() { return Bjackstate; }
        public GameStates get_Game_State() { return gamestate; }

        //goes through steps dicussed as a group
        public void TheGame()
        {
            while (true) { 
            //step1: wait for players.  Game state set to waiting
            set_Game_State(GameStates.Waiting);

            /*SERVER CHECKS FOR CONNECTIONS 
              if(no connections) break;*/


            //step2: Get deck...get from Hayden's Deck class!!
            Deck shuffled_deck = new Deck();


            //step3: Let players bet. Set Bjack state to betting and game state to playing
            set_Bjack_State(BlackjackStates.Betting);
            set_Game_State(GameStates.Playing);

            //iterate through the list of players and ask for their bet amount
            foreach (BlackjackPlayer player in blackjackPlayers){
               Console.Write("place your bet");
                    player.IndicateBet();
                    //Timer(); //goes to timer class, which loops over 30 sec and then terminates. Gives user 30 secs
            }
                Timer(); //wait for all bets or 30sec
            


            //step4:deal cards blackjack state changes to dealing.  CHECK DEALER HAND AND IF 21 GAMEOVER EVERYONE LOST
            set_Bjack_State(BlackjackStates.Dealing);
        
            foreach (BlackjackPlayer player in blackjackPlayers) {

                Card card1 = shuffled_deck.DealCard();
                Card card2 = shuffled_deck.DealCard();
                player.DealtCards(card1);
                player.DealtCards(card2);
          
            }

            //Dealer gets cards and is checked for 21 
            Card dealercard1 = shuffled_deck.DealCard();
            Card dealercard2 = shuffled_deck.DealCard();
            Dealer.Add(shuffled_deck.DealCard()); Dealer.Add(shuffled_deck.DealCard());
            //use Hayden's class to calculate numerical value
            int DealerAmount = CardHelper.CountHand(Dealer);


        
            //step5: each player hits or stays  bjack state is now userplayig
            set_Bjack_State(BlackjackStates.UserPlaying);

                foreach (BlackjackPlayer player in blackjackPlayers) {
                   /* Boolean done = false;
                    do {
                        player.IndicatePlaying();
                    } while (done == false);
                    //wait for player or 30sec
                    Timer(); will need to be changed so users can hit or stay according to time*/
                }

                    //step6:dealer hits until 17 or over
                    while (DealerAmount >= 17 && DealerAmount < 21) {
                        Dealer.Add(shuffled_deck.DealCard());
                        DealerAmount =/*get amount from Hayden's cardhelper class */
                }

                    //setp7: Winnings are distributed  if over 21 no winnings
                    foreach (BlackjackPlayer player in blackjackPlayers)
                    {
                        //Calculate numerical value of player hand
                        int PlayersHand = CardHelper.CountHand(player.GetCards());

                        //players hand and compare to dealers hand
                        if (PlayersHand > DealerAmount) { /*distribute winnings*/}
                        else if (PlayersHand < DealerAmount) { /*take bet amount*/}
                        else { /*player takes only what he/she be. No more no less*/}
                    }

                    //step8:Round Over game state set to waiting again
                    set_Game_State(GameStates.Waiting);
                }//ends beginning while loop
            }

        }


          
         

        
        

    }
}
