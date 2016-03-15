using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;

namespace Intermediate.Communications
{
    class Server
    {
        public void SetConnection(/*parameters for connection*/)
        {

        }

        public void SetGameBalance(int Balance)
        {
            //Set balance for game that is equal to current player balance
        }

        public void SetSeat(int seatNumber)
        {
            //Set seat number for game, mainly for GUI and turn purposes
        }

        public void InitiateTurn(bool isTurn)
        {
            //Send isTurn to client 
        }

        public void IndicateWin(bool win)
        {
            //Send indication of win or not to client
        }

        public void AdjastGameBalance(int balanceChange)
        {
            //Send balanceChange to client that adjusts the game balance 
            //This might be able to be done in the client 
        }
    }
}
