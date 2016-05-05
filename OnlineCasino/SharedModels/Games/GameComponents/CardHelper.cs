using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SharedModels.GameComponents
{
    public static class CardHelper
    {
        public static int CountHand(List<Card> hand)
        {
            int handCount = 0;
            List<int> valueList = new List<int>();
            foreach(Card card in hand)
            {
                int cardValue;

                if (card.Rank == CardRank.Ace)
                    cardValue = 1;
                else if ((int)card.Rank <= 10)
                    cardValue = (int)card.Rank;
                else
                    cardValue = 10;

                handCount += cardValue;
            }

            foreach(Card card in hand.Where(c => c.Rank == CardRank.Ace))
            {
                if (handCount + 10 <= 21)
                    handCount += 10;
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
        public static int HandValue(List<Card> hand, List<Card> tablecards)
        {

            CardSuit suit = CardSuit.Clubs;
            bool king = false;
            bool queen = false;
            bool jack = false;
            bool ten = false;
            bool nine = false;
            bool eight = false;
            bool seven = false;
            bool six = false;
            bool five = false;
            bool four = false;
            bool three = false;
            bool two = false;
            bool ace = false;
            bool samesuit = false;
            string FourOfAKind= "null";
            string ThreeOfAKind = "null";
            string TwoPair = "null";
            string OnePair = "null";

            int temp = 0;
            //RoyalFlush check
            foreach (Card card in hand)
            {




                if (card.Rank == CardRank.King)
                {
                    if (king == true)
                    {
                        if (OnePair == "king") { if (ThreeOfAKind == "king") { FourOfAKind = "king"; } else { ThreeOfAKind = "king"; } }
                        else if (OnePair == "null") { OnePair = "king"; }
                        else if (OnePair != "null") { TwoPair = "king" + OnePair; }
                    }
                    king = true;
                }
                else if (card.Rank == CardRank.Queen)
                {
                    if (queen == true)
                    {
                        if (OnePair == "queen") { if (ThreeOfAKind == "queen") { FourOfAKind = "queen"; } else { ThreeOfAKind = "queen"; } }
                        else if (OnePair == "null") { OnePair = "queen"; }
                        else if (OnePair != "null") { TwoPair = "queen" + OnePair; }
                    }
                    queen = true;
                }
                else if (card.Rank == CardRank.Jack)
                {
                    if (jack == true)
                    {
                        if (OnePair == "jack") { if (ThreeOfAKind == "jack") { FourOfAKind = "jack"; } else { ThreeOfAKind = "jack"; } }
                        else if (OnePair == "null") { OnePair = "jack"; }
                        else if (OnePair != "null") { TwoPair = "jack" + OnePair; }
                    }
                    jack = true;
                }
                else if (card.Rank == CardRank.Ten)
                {
                    if (ten == true)
                    {
                        if (OnePair == "ten") { if (ThreeOfAKind == "ten") { FourOfAKind = "ten"; } else { ThreeOfAKind = "ten"; } }
                        else if (OnePair == "null") { OnePair = "ten"; }
                        else if (OnePair != "null") { TwoPair = "ten" + OnePair; }
                    }
                    ten = true;
                }
                else if (card.Rank == CardRank.Nine)
                {
                    if (nine == true)
                    {
                        if (OnePair == "nine") { if (ThreeOfAKind == "nine") { FourOfAKind = "nine"; } else { ThreeOfAKind = "nine"; } }
                        else if (OnePair == "null") { OnePair = "nine"; }
                        else if (OnePair != "null") { TwoPair = "nine" + OnePair; }
                    }
                    nine = true;
                }
                else if (card.Rank == CardRank.Eight)
                {
                    if (eight == true)
                    {
                        if (OnePair == "eight") { if (ThreeOfAKind == "eight") { FourOfAKind = "eight"; } else { ThreeOfAKind = "eight"; } }
                        else if (OnePair == "null") { OnePair = "eight"; }
                        else if (OnePair != "null") { TwoPair = "eight" + OnePair; }
                    }
                    eight = true;
                }
                else if (card.Rank == CardRank.Seven)
                {
                    if (seven == true)
                    {
                        if (OnePair == "seven") { if (ThreeOfAKind == "seven") { FourOfAKind = "seven"; } else { ThreeOfAKind = "seven"; } }
                        else if (OnePair == "null") { OnePair = "seven"; }
                        else if (OnePair != "null") { TwoPair = "seven" + OnePair; }
                    }
                    seven = true;
                }
                else if (card.Rank == CardRank.Six)
                {
                    if (six == true)
                    {
                        if (OnePair == "six") { if (ThreeOfAKind == "six") { FourOfAKind = "six"; } else { ThreeOfAKind = "six"; } }
                        else if (OnePair == "null") { OnePair = "six"; }
                        else if (OnePair != "null") { TwoPair = "six" + OnePair; }
                    }
                    six = true;
                }
                else if (card.Rank == CardRank.Five)
                {
                    if (five == true)
                    {
                        if (OnePair == "five") { if (ThreeOfAKind == "five") { FourOfAKind = "five"; } else { ThreeOfAKind = "five"; } }
                        else if (OnePair == "null") { OnePair = "five"; }
                        else if (OnePair != "null") { TwoPair = "five" + OnePair; }
                    }
                    five = true;
                }
                else if (card.Rank == CardRank.Four)
                {
                    if (four == true)
                    {
                        if (OnePair == "four") { if (ThreeOfAKind == "four") { FourOfAKind = "four"; } else { ThreeOfAKind = "four"; } }
                        else if (OnePair == "null") { OnePair = "four"; }
                        else if (OnePair != "null") { TwoPair = "four" + OnePair; }
                    }
                    four = true;
                }
                else if (card.Rank == CardRank.Three)
                {
                    if (three == true)
                    {
                        if (OnePair == "three") { if (ThreeOfAKind == "three") { FourOfAKind = "three"; } else { ThreeOfAKind = "three"; } }
                        else if (OnePair == "null") { OnePair = "three"; }
                        else if (OnePair != "null") { TwoPair = "three" + OnePair; }
                    }
                    three = true;
                }
                else if (card.Rank == CardRank.Two)
                {
                    if (two == true)
                    {
                        if (OnePair == "two") { if (ThreeOfAKind == "two") { FourOfAKind = "two"; } else { ThreeOfAKind = "two"; } }
                        else if (OnePair == "null") { OnePair = "two"; }
                        else if (OnePair != "null") { TwoPair = "two" + OnePair; }
                    }
                    two = true;
                }
                else if (card.Rank == CardRank.Ace)
                {
                    if (ace == true)
                    {
                        if (OnePair == "ace") { if (ThreeOfAKind == "ace") { FourOfAKind = "ace"; } else { ThreeOfAKind = "ace"; } }
                        else if (OnePair == "null") { OnePair = "ace"; }
                        else if (OnePair != "null") { TwoPair = "ace" + OnePair; }
                    }
                    ace = true;
                }


            }
            foreach (Card card in tablecards)
            {




                if (card.Rank == CardRank.King)
                {
                    if (king == true)
                    {
                        if (OnePair == "king") { if (ThreeOfAKind == "king") { FourOfAKind = "king"; } else { ThreeOfAKind = "king"; } }
                        else if (OnePair == "null") { OnePair = "king"; }
                        else if (OnePair != "null") { TwoPair = "king" + OnePair; }
                    }
                    king = true;
                }
                else if (card.Rank == CardRank.Queen)
                {
                    if (queen == true)
                    {
                        if (OnePair == "queen") { if (ThreeOfAKind == "queen") { FourOfAKind = "queen"; } else { ThreeOfAKind = "queen"; } }
                        else if (OnePair == "null") { OnePair = "queen"; }
                        else if (OnePair != "null") { TwoPair = "queen" + OnePair; }
                    }
                    queen = true;
                }
                else if (card.Rank == CardRank.Jack)
                {
                    if (jack == true)
                    {
                        if (OnePair == "jack") { if (ThreeOfAKind == "jack") { FourOfAKind = "jack"; } else { ThreeOfAKind = "jack"; } }
                        else if (OnePair == "null") { OnePair = "jack"; }
                        else if (OnePair != "null") { TwoPair = "jack" + OnePair; }
                    }
                    jack = true;
                }
                else if (card.Rank == CardRank.Ten)
                {
                    if (ten == true)
                    {
                        if (OnePair == "ten") { if (ThreeOfAKind == "ten") { FourOfAKind = "ten"; } else { ThreeOfAKind = "ten"; } }
                        else if (OnePair == "null") { OnePair = "ten"; }
                        else if (OnePair != "null") { TwoPair = "ten" + OnePair; }
                    }
                    ten = true;
                }
                else if (card.Rank == CardRank.Nine)
                {
                    if (nine == true)
                    {
                        if (OnePair == "nine") { if (ThreeOfAKind == "nine") { FourOfAKind = "nine"; } else { ThreeOfAKind = "nine"; } }
                        else if (OnePair == "null") { OnePair = "nine"; }
                        else if (OnePair != "null") { TwoPair = "nine" + OnePair; }
                    }
                    nine = true;
                }
                else if (card.Rank == CardRank.Eight)
                {
                    if (eight == true)
                    {
                        if (OnePair == "eight") { if (ThreeOfAKind == "eight") { FourOfAKind = "eight"; } else { ThreeOfAKind = "eight"; } }
                        else if (OnePair == "null") { OnePair = "eight"; }
                        else if (OnePair != "null") { TwoPair = "eight" + OnePair; }
                    }
                    eight = true;
                }
                else if (card.Rank == CardRank.Seven)
                {
                    if (seven == true)
                    {
                        if (OnePair == "seven") { if (ThreeOfAKind == "seven") { FourOfAKind = "seven"; } else { ThreeOfAKind = "seven"; } }
                        else if (OnePair == "null") { OnePair = "seven"; }
                        else if (OnePair != "null") { TwoPair = "seven" + OnePair; }
                    }
                    seven = true;
                }
                else if (card.Rank == CardRank.Six)
                {
                    if (six == true)
                    {
                        if (OnePair == "six") { if (ThreeOfAKind == "six") { FourOfAKind = "six"; } else { ThreeOfAKind = "six"; } }
                        else if (OnePair == "null") { OnePair = "six"; }
                        else if (OnePair != "null") { TwoPair = "six" + OnePair; }
                    }
                    six = true;
                }
                else if (card.Rank == CardRank.Five)
                {
                    if (five == true)
                    {
                        if (OnePair == "five") { if (ThreeOfAKind == "five") { FourOfAKind = "five"; } else { ThreeOfAKind = "five"; } }
                        else if (OnePair == "null") { OnePair = "five"; }
                        else if (OnePair != "null") { TwoPair = "five" + OnePair; }
                    }
                    five = true;
                }
                else if (card.Rank == CardRank.Four)
                {
                    if (four == true)
                    {
                        if (OnePair == "four") { if (ThreeOfAKind == "four") { FourOfAKind = "four"; } else { ThreeOfAKind = "four"; } }
                        else if (OnePair == "null") { OnePair = "four"; }
                        else if (OnePair != "null") { TwoPair = "four" + OnePair; }
                    }
                    four = true;
                }
                else if (card.Rank == CardRank.Three)
                {
                    if (three == true)
                    {
                        if (OnePair == "three") { if (ThreeOfAKind == "three") { FourOfAKind = "three"; } else { ThreeOfAKind = "three"; } }
                        else if (OnePair == "null") { OnePair = "three"; }
                        else if (OnePair != "null") { TwoPair = "three" + OnePair; }
                    }
                    three = true;
                }
                else if (card.Rank == CardRank.Two)
                {
                    if (two == true)
                    {
                        if (OnePair == "two") { if (ThreeOfAKind == "two") { FourOfAKind = "two"; } else { ThreeOfAKind = "two"; } }
                        else if (OnePair == "null") { OnePair = "two"; }
                        else if (OnePair != "null") { TwoPair = "two" + OnePair; }
                    }
                    two = true;
                }
                else if (card.Rank == CardRank.Ace)
                {
                    if (ace == true)
                    {
                        if (OnePair == "ace") { if (ThreeOfAKind == "ace") { FourOfAKind = "ace"; } else { ThreeOfAKind = "ace"; } }
                        else if (OnePair == "null") { OnePair = "ace"; }
                        else if (OnePair != "null") { TwoPair = "ace" + OnePair; }
                    }
                    ace = true;
                }


            }
            int samecards = 0;
            int samecards2 = 0;
            int samecards3 = 0;
            CardSuit suit2= CardSuit.Clubs;
            CardSuit suit3 = CardSuit.Clubs;
            
            foreach (Card card in hand) {
                if (temp == 0)
                { suit = card.Suit; temp = 1; }
                if (temp == 1)
                { if (card.Suit != suit) { suit2 = card.Suit;temp++; } }
                if(temp == 3)
                { if(card.Suit != suit && card.Suit != suit2) { suit3 = card.Suit;temp++; } }


                if (card.Suit == suit)
                {
                    samecards++;
                    }
                else if (card.Suit == suit2)
                {
                    samecards2++;
                }
                else if (card.Suit == suit3)
                {
                    samecards3++;
                }
            }

          

            foreach (Card card in tablecards)
            {

                if (card.Suit == suit)
                {
                    samecards++;
                }
                else if (card.Suit == suit2)
                {
                    samecards2++;
                }
                else if (card.Suit == suit3)
                {
                    samecards3++;
                }
            }
        
            if (samecards >= 5 || samecards2 >=5 ||samecards3 >=5)
                samesuit = true;
            else
                samesuit = false;
            //royal flush
            if (king && queen && jack && ten && samesuit)
            { return 22; }

            //straight flush
           
            if( (nine && eight && seven && six &&five && samesuit) || ( eight && seven && six && five && four && samesuit)  ||  (seven && six && five && four && three && samesuit) || (six && five && four && three && two && samesuit) || (six && five && four && three && two&& ace && samesuit))
            {
                return 21;
            }
            if (FourOfAKind != "null")
                return 20;
            if (OnePair != "null" && ThreeOfAKind !="null" && OnePair != ThreeOfAKind)
                return 19;
            if (samesuit)
                return 18;
            if ((nine && eight && seven && six && five) || (eight && seven && six && five && four) || (seven && six && five && four && three) || (six && five && four && three && two) || (six && five && four && three && two && ace))
                return 17;
            if (ThreeOfAKind != "null")
                return 16;
            if (TwoPair != "null")
                return 15;
            if (OnePair != "null")
                return 14;
            if (ace)
                return 13;
            if (king)
                return 12;
            if (queen)
                return 11;
            if (jack)
                return 10;
            if (ten)
                return 9;
            if (nine)
                return 8;
            if (eight)
                return 7;
            if (seven)
                return 6;
            if (six)
                return 5;
            if (five)
                return 4;
            if (four)
                return 3;
            if (three)
                return 2;
            if (two)
                return 1;
            


            return 0; 
        }
    }
}
