using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Players
{
    public abstract class CardPlayer : Player
    {
        public List<Card> Hand;

        public CardPlayer(SharedModels.Players.Player P)
            :base(P)
        {
            Hand = new List<Card>();
        }

        public void UpdateHand(Card c)
        {
            Hand.Add(c);
        }

        public void RefreshHand()
        {
            Hand = new List<Card>();

            // Deal two cards
        }


    }
}
