using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;

namespace ServerLogic.Games.GameComponents
{
    public class Deck
    {
        public List<Card> deck;
        private Random rand = new Random();

        public Deck()
        {
            deck = new List<Card>();

            for(int i = 1; i < 14; i++)
            {
                CardRank rank = (CardRank)i;
                for(int j = 0; j < 4; j++)
                {
                    Card card = new Card((CardSuit)j, rank);
                    
                    deck.Add(card);
                }
            }

            Shuffle();
            Shuffle();
            Shuffle();

            foreach(var card in deck)
            {
                Console.Out.Write(card.Rank + " of " + card.Suit + "\n");
            }
        }

        public void Shuffle()
        {
            for (var i = 0; i < deck.Count; i++)
                Swap(i, rand.Next(i, deck.Count));
        }
        
        public void Swap(int i, int j)
        {
            var t = deck[i];
            deck[i] = deck[j];
            deck[j] = t;
        }

        public int Count()
        {
            return deck.Count();
        }
    }
}
