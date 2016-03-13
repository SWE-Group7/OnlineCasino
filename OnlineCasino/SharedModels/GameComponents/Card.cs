using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SharedModels.GameComponents
{
    public class Card
    {
        public CardSuit Suit;
        public CardRank Rank;

        public Card(CardSuit suit, CardRank rank)
        {
            this.Suit = suit;
            this.Rank = rank;
        }

        public CardSuit getCardSuit()
        {
            return this.Suit;
        }
    }
}
