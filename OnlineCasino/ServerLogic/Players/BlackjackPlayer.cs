using ServerLogic.Games;
using ServerLogic.Games.GameComponents;
using SharedModels.Connection.Enums;
using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP = SharedModels.Players;

namespace ServerLogic.Players
{
    public class BlackjackPlayer : Player
    {

        public const GameTypes GameType = GameTypes.Blackjack;
        public readonly new Blackjack CurrentGame;
        private int _CardCount = 0;
        public int CardCount
        {
            get
            {
                if (_CardCount == 0)
                    _CardCount = CardHelper.CountHand(Cards);

                return _CardCount;

            }
        }
        private List<Card> Cards;

        public BlackjackPlayer(User user, Blackjack game, int seat, int buyIn)
            : base(user, game, seat, buyIn)
        {
            Cards = new List<Card>();
        }


        public List<Card> GetCards()
        {
            List<Card> CardsCopy = new List<Card>();
            foreach (Card C in Cards)
                CardsCopy.Add(new Card(C.Suit, C.Rank));
            
            return CardsCopy;
        }
        public void DealCard(Card card)
        {
            Cards.Add(card);
            _CardCount = CardHelper.CountHand(Cards);
        }
        public void ClearCards()
        {
            Cards.Clear();
            _CardCount = 0;
        }
        public int GetCardCount()
        {
            return CardCount;
        }
      

    

    }

   

    
}