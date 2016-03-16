using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Games.GameComponents;
using SharedModels.GameComponents;


namespace ServerLogic.Games
{
    public enum BlackjackStates {
        //flags for states
       Betting, Dealing,UserPlaying,GainsorLoses}

    class Blackjack : Game
    {
        private List<Card> Dealer;//keeps track of dealer hand
         
        //enum variables to define states within the game
        BlackjackStates current_bjack_state;
        //GameStates current_game_state;


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

        //methods that can be called to set states and get states. Get states will be used to verify if ok for a new player to join
        public void set_Game_State(GameStates state) { state=GameStates.Waiting; }
        public void set_Bjack_State(BlackjackStates bjackstate) { current_bjack_state = bjackstate; }

        public BlackjackStates get_Bjack_State() { return current_bjack_state; }
        public GameStates get_Game_State() { return ??; }

        //goes through steps dicussed as a group
        public void TheGame()
        {
            //step1: wait for players.  Game state set to waiting
            set_Game_State(GameStates.Waiting);
            /*SERVER CHECKS FOR CONNECTIONS 
              while(connections being made){gamestate=waiting}*/


            //step2: Get deck...get from Hayden's Deck class!!
            Deck shuffled_deck = new Deck();


            //step3: Let players bet  Set Bjack state to betting and game state to playing
            set_Bjack_State(BlackjackStates.Betting);
            set_Game_State(GameStates.Playing);

            //iterate through the list of players and ask for their bet amount
            foreach (BlackjackPlayer player in blackjackPlayers){
               Console.Write("place your bet");
                player.IndicateBet();
            }

            //wait fo all bets or 30sec


            //step4:deal cards blackjack state changes to dealing.  CHECK DEALER HAND AND IF 21 GAMEOVER EVERYONE LOST
            set_Bjack_State(BlackjackStates.Dealing);
        
            foreach (BlackjackPlayer player in blackjackPlayers) {

                Card card1 = shuffled_deck.DealCard();
                Card card2 = shuffled_deck.DealCard();
                player.DealCard(card1);
                player.DealCard(card2);
       
            }

            //Dealer gets cards and is checked for 21 
            Card dealercard1 = shuffled_deck.DealCard();
            Card dealercard2 = shuffled_deck.DealCard();
            Dealer.Add(shuffled_deck.DealCard()); Dealer.Add(shuffled_deck.DealCard());
            //use Hayden's CardHelper class to add cards and return bumerical value

        
            //step5: each player hits or stays  bjack state is now userplayig
            set_Bjack_State(BlackjackStates.UserPlaying);

            foreach (BlackjackPlayer player in blackjackPlayers) {
                Boolean done = false;
                do {
                    player.IndicatePlay(); //will probably want the method to return true or false to know where or not the loop continues
                } while (done == false);
                //wait for player or 30sec


                //step6:dealer hits until 17 or over
                while (DealerAmount => 17 && DealerAmount < 21) {
                    Dealer.Add(shuffled_deck.DealCard());
                    DealerAmount=/*get amount from Hayden's cardhelper class */
                }

                //setp7: Winnings are distributed  if over 21 no winnings
                foreach (BlackjackPlayer players in blackjackPlayers)
                {
                    //add players hand and compare to dealers hand
                    if (PlayersHand > DealerAmount) { /*distribute winnings*/}
                    else if (PlayersHand < DealerAmount) { /*take bet amount*/}
                    else { /*player takes only what he/she be. No more no less*/}

                }
          
              //step8:Round Over
               
            }

        }


          
         

        
        

    }
}
