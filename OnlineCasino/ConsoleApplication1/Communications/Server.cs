using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediate.Communications
{
    public static class Server
    {
        public static void SetConnection()
        {

        }

        public static void SetTable(int tableNumber)
        {

        }

        public static void SetGameBalance(int Balance)
        {

        }

        public static void SetSeat(int seatNumber)
        {

        }

        public static void SetPlayers()
        {

        }

        public static void StartGame()
        {

        }

        public static void DealCards()
        {
            //Deals out cards from the deck to the players and dealer
        }

        public static bool CheckTurn()
        {
            bool turn = true;
            return turn;
        }

        public static void NextTurn()
        {
            //Sets to next player's turn after Stay()
        }

        public static void ShowDealer()
        {
            //not sure if this should actually be in the server or done in the client
            //Shows dealer hand
        }

        public static bool CheckWin()
        {
            bool win = false;
            return win;
        }

        public static int AdjustGameBalance()
        {
            var gameBalance = 0;
            return gameBalance;
        }

        public static void SetBalance(int balance)
        {

        }

        public static void EndConnection()
        {

        }
    }
}
