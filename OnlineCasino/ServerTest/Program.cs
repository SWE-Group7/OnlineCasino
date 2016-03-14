using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLogic.Games.GameComponents;
using SharedModels.GameComponents;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck d = new Deck();
            d.PrintDeck();
            Console.Out.Write(d.Count() + "\n");
            Card card = d.getTop();
            d.AddCard(card);
            card = d.DealCard();
            d.PrintDeck();
            Console.Out.Write(d.Count() + "\n");
            d.AddCard(card);
            d.PrintDeck();
            Console.Out.Write(d.Count() + "\n");
            Console.Read();
        }
    }
}
