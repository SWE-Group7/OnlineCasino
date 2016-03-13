using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediate.Communications
{
    class Server
    {
        public void SetConnection()
        {

        }

        public void SetTable(int tableNumber)
        {

        }

        public void SetGameBalance(int Balance)
        {

        }

        public void SetSeat(int seatNumber)
        {

        }

        public void SetPlayers()
        {

        }

        public void StartGame()
        {

        }

        public void DealCards()
        {
            //Deals out cards from the deck to the players and dealer
        }

        public bool CheckTurn()
        {
            bool turn = true;
            return turn;
        }

        public void NextTurn()
        {
            //Sets to next player's turn after Stay()
        }

        public void ShowDealer()
        {
            //not sure if this should actually be in the server or done in the client
            //Shows dealer hand
        }

        public bool CheckWin()
        {
            bool win = false;
            return win;
        }

        public int AdjastGameBalance()
        {
            var gameBalance = 0;
            return gameBalance;
        }

        public void SetBalance(int balance)
        {

        }

        public void EndConnection()
        {

        }
    }
}
