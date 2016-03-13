using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.GameComponents;

namespace ServerLogic.Games.GameComponents
{
    class Deck
    {
        List<Card> deck;

        public Deck()
        {
            deck = new List<Card>();

            for(int i = 0; i < 13; i++)
            {
                CardRank rank = (CardRank)i;
                for(int j = 0; j < 4; j++)
                {
                    Card card = new Card((CardSuit)j, rank);
                    s
                    deck.Add(card);
                }
            }

            Shuffle(deck);
        }

        public static void Shuffle<Card>(this List<Card> deck, Random rand)
        {
            for (var i = 0; i < deck.Count; i++)
                Swap(deck, i, rand.Next(i, deck.Count));
        }

        public static void Swap<Card>(this List<Card> deck, int i, int j)
        {
            var t = deck[i];
            deck[i] = deck[j];
            deck[j] = t;
        }
    }
}
