using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;

namespace Intermediate.Communications
{
    public class Client
    {
        public void OpenConnection()
        {

        }

        public void CheckConnection()
        {

        }

        public void ChooseGame()
        {

        }

        public int GetBalance()
        {
            var balance = 0;
            return balance;
        }

        public int GetSeat()
        {
            var seat = 0;
            return seat;
        }

        public void PlaceBet(int bet)
        {

        }

        public bool CheckBlackJ(Card card1, Card card2)
        {
            bool BlackJack = true;
            return BlackJack;
        }

        public void Hit()
        {
            //Deal card to player
        }

        public void Stay()
        {
            //Move to next Player
        }

        public void EndGame()
        {

        }
    }
}
