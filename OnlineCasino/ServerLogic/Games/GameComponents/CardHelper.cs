using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;



namespace ServerLogic.Games.GameComponents
{
    public static class CardHelper
    {
        public static int CountHand(List<Card> hand)
        {
            int handCount = 0;
            List<int> valueList = new List<int>();
            foreach(Card card in hand)
            {
                int temp = handCount;
                int cardValue = (int)card.Rank;

                if (cardValue >= 10)
                    cardValue = 10;
                if (cardValue == 1)
                    cardValue = 11;

                valueList.Add(cardValue);
                temp += cardValue;

                while ((temp > 21) && (valueList.Exists(cards => cards == 11)))
                    temp = temp - 10; 

                handCount = temp;              
            }
            return handCount;
        }

        public static void PrintHand(List<Card> hand)
        {
            foreach(Card card in hand)
            {
                Console.Out.Write(card.Rank + " of " + card.Suit + "\n");
            }
        }
    }
}
