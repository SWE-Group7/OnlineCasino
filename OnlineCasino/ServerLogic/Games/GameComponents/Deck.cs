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
        }

        //Just returns the top card, does not deal it from the deck
        //Mainly for testing purposes
        public Card getTop()
        {
            Card card = deck[0];
            return card;
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

        public Card DealCard()
        {
            Card dealt;
            dealt = deck[0];
            deck.RemoveAt(0);

            return dealt;
        }

        //Adds card back into deck if it does not already exist
        //Adds to bottom of deck
        public void AddCard(Card card)
        {
            bool contain = deck.Exists(cards => cards == card);
            if (contain == true)
                Console.Out.Write("The " + card.Rank + " of " + card.Suit + " is already in the deck\n");
            else
            {
                deck.Add(card);
                Console.Out.Write(card.Rank + " of " + card.Suit + " added\n");
            }
        }

        //Prints out list of the deck
        public void PrintDeck()
        {
            foreach (var card in deck)
            {
                Console.Out.Write(card.Rank + " of " + card.Suit + "\n");
            }
        }
    }
}
