using SharedModels.Connection.Enums;
using SharedModels.GameComponents;
using SharedModels.Games.Enums;
using SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Games.Events
{
    [Serializable]
    public abstract class GameEvent
    {

        public readonly byte Event;
        public readonly Player Player;
        public readonly byte Seat;
        public readonly int Num;
        public readonly string Text;

        public readonly byte[] Seats;
        public readonly int[] Winnings;

        internal GameEvent(int gameEvent, int seat, int num, string text, byte[] seats, int[] winnings, Player player)
        {
            this.Event = (byte) gameEvent;
            this.Seat = (byte) seat;
            this.Player = player;
            this.Num = num;
            this.Text = text;
            this.Seats = seats;
            this.Winnings = winnings;
        }



    }

    [Serializable]
    public class BlackjackEvent : GameEvent
    {
        public readonly Card[][] Cards;

        private BlackjackEvent(Builder b)
            : base((int)b.Event, b.Seat, b.Num, b.Text, b.Seats, b.Winnings, b.Player)
        {
            Cards = b.Cards;
        }

        public static BlackjackEvent StartGame()
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.StartGame;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent Deal(Dictionary<int, Card[]> cards)
        {
            Builder b = new Builder();
            b.Cards = new Card[cards.Count][];
            b.Seats = new byte[cards.Count];
            b.Event = BlackjackEvents.Deal;

            int i = 0;
            foreach (var pair in cards)
            {
                b.Cards[i] = pair.Value;
                b.Seats[i] = (byte)pair.Key;
                i++;
            }

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent ShowDealer(Card card)
        {
            Builder b = new Builder();
            b.Cards = new Card[1][];
            b.Cards[0] = new Card[] { card };
            b.Event = BlackjackEvents.ShowDealer;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent Payout(Dictionary<int, int> seatToWinnings)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.Payout;
            b.Seats = new byte[seatToWinnings.Count];
            b.Winnings = new int[seatToWinnings.Count];

            int i = 0;
            foreach(var pair in seatToWinnings)
            {
                b.Seats[i] = (byte) pair.Key;
                b.Winnings[i] = pair.Value;
                i++;
            }

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent ChangeState(BlackjackStates state)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.ChangeGameState;
            b.Num = (int)state;

            return new BlackjackEvent(b);            
        }
        
        public static BlackjackEvent PlayerTurn(int seat)
        {
            Builder b = new Builder();
            b.Seat = seat;
            b.Event = BlackjackEvents.PlayerTurn;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerHit(int seat, Card card)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerHit;
            b.Seat = seat;
            b.Cards = new Card[1][];
            b.Cards[0] = new Card[] { card };

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerStay(int seat)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerStay;
            b.Seat = seat;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerDouble(int seat)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerDouble;
            b.Seat = seat;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerBet(int seat, int bet)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerBet;
            b.Seat = seat;
            b.Num = bet;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerBust(int seat)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerBust;

            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerJoin(Player player)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerJoin;
            b.Player = player;
            return new BlackjackEvent(b);
        }
        public static BlackjackEvent PlayerQuit(int seat)
        {
            Builder b = new Builder();
            b.Event = BlackjackEvents.PlayerQuit;
            b.Seat = seat;

            return new BlackjackEvent(b);
        }
        private class Builder
        {
            public BlackjackEvents Event;
            public Player Player;
            public int Seat;
            public int Num;
            public string Text;
            public Card[][] Cards;
            public byte[] Seats;
            public int[] Winnings;

        }
        
    }

    
}
