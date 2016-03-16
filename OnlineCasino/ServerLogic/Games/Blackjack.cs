using ServerLogic.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Games.GameComponents;

namespace ServerLogic.Games
{
    public enum BlackjackStates {
        //flags for states
       Betting, Dealing,UserPlaying,GainsorLoses}

    class Blackjack : Game
    {
        //enum variables to define states within the game
        BlackjackStates current_bjack_state;
        GameStates current_game_state;


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
        public void set_Game_State(GameStates state) { current_game_state = state; }
        public void set_Bjack_State(BlackjackStates bjackstate) { current_bjack_state = bjackstate; }

        public BlackjackStates get_Bjack_State() { return current_bjack_state; }
        public GameStates get_Game_State() { return current_game_state; }

        //goes through steps dicussed as a group
        public void TheGame()
        {
            //step1: wait for players.  Game state set to waiting
            set_Game_State(GameStates.Waiting);
            /*SERVER CHECKS FOR CONNECTIONS 
              while(connections being made){gamestate=waiting}*/


            //step2: Get deck...get from Hayden's Deck class!!
            Deck deck = new Deck();

            //step3: Let players bet  Set Bjack state to betting and game state to playing
            set_Bjack_State(BlackjackStates.Betting);
            set_Game_State(GameStates.Playing);

            //iterate through the list of players and ask for their bet amount
            foreach (BlackjackPlayer player in blackjackPlayers){
               Console.Write("place your bet");
               float user_bet = float.Parse(Console.ReadLine());
                //where does user bet go??
                player.set_User_Bet(user_bet);//User_bet in BlackjackPlayer.cs
            }

            //step4:deal cards blackjack state changes to dealing.  CHECK DEALER HAND AND IF 21 GAMEOVER EVERYONE LOST
            set_Bjack_State(BlackjackStates.Dealing);
            foreach (BlackjackPlayer blackjackplay in blackjackPlayers) {

                /* Deckclassobj obj= new Deckclassobj;
                   card1=obj.DealCardfunc;
                   card2=obj.DealCardfunc;
                blackjackplay.Someuserhandlist=card1 + card2*/
            }

            //step5: each player hits or stays  bjack state is now userplayig
            set_Bjack_State(BlackjackStates.UserPlaying);

            foreach (BlackjackPlayer theplayers in blackjackPlayers) {
                Boolean done = false;
                do { Console.Write("hit or stay??");
                    string userchoice = Console.ReadLine();

                    if (userchoice == "hit") { /* add to userhand list */}
                    else done = true;

                } while (done == false);

          //step6:dealer hits until 17 or over
             //is dealer considered another player or will dealer class be needed??

         //setp7: Winnings are distributed  if over 21 no winnings
          

          


            }

        }


          
         

        
        

    }
}
