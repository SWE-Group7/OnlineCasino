using ClientLogic.Players;
using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLogic.Game_GUIs
{
    public abstract class CardGameGUI : GameGUI
    {
        public Bitmap CardImage;
        protected Deck Deck;

        public decimal buyIn;
        public decimal bet;
        protected int yourCardX;
        protected int yourCardY;
        protected int yourCardsCount = 0;
        protected int yourCardOffset;

        protected int dealerCardX;
        protected int dealerCardY;
        protected int dealerCardsCount = 0;
        protected int dealerCardOffset;

        protected int otherPlayerCardX;
        protected int otherPlayerCardY;
        protected int otherPlayerCardCount = 0;
        protected int otherPlayerCardOffset = 0;
        protected int otherPlayerOffset = 0;
        protected int otherPlayerCount = 0;

        protected int cardHeight = 150;
        protected int cardWidth = 120;

        protected void YourHand_Paint(object sender, System.Windows.Forms.PaintEventArgs e, CardPlayer You)
        {
            e.Graphics.DrawLine(Pens.Black, new Point(0, clientHeight - cardHeight - 60), new Point(1500, clientHeight - cardHeight - 60));

            e.Graphics.DrawString("Buy In: $" + buyIn, new Font("Segoe UI", 12), Brushes.White, new Point(100, clientHeight - cardHeight));
            e.Graphics.DrawString("   Bet: $" + bet, new Font("Segoe UI", 12), Brushes.White, new Point(106, clientHeight - cardHeight + 20));

            yourCardOffset = (You.Hand.Count * (cardWidth + 20)) / 2;
            foreach (Card c in You.Hand)
            {
                CardImage = Deck.CardImage(c.Suit, c.Rank);
                yourCardX += (yourCardsCount * cardWidth + yourCardsCount * 20);

                if (CardImage != null)
                {
                    e.Graphics.DrawImage(CardImage, new Rectangle(yourCardX, yourCardY, cardWidth, cardHeight));
                }

                yourCardsCount--;
                yourCardX = clientWidth / 2 - yourCardOffset;
            }
            yourCardsCount = You.Hand.Count - 1;
        }

        protected void OtherPlayerHands_Paint(object sender, System.Windows.Forms.PaintEventArgs e, List<CardPlayer> OtherPlayers)
        {
            otherPlayerCount = OtherPlayers.Count;
            bool leftSide = true;
            foreach (BlackjackPlayer p in OtherPlayers)
            {
                otherPlayerCardCount = p.Hand.Count;

                if (otherPlayerCount % 2 == 0)
                {
                    otherPlayerCardY = 400;
                    if (otherPlayerCount == 2) { otherPlayerCardX = 30; leftSide = true; }
                    else { otherPlayerCardX = clientWidth - cardWidth - 30; leftSide = false; }
                }
                else
                {
                    otherPlayerCardY = 100;
                    if (otherPlayerCount == 1) { otherPlayerCardX = 30; leftSide = true; }
                    else { otherPlayerCardX = clientWidth - cardWidth - 30; leftSide = false; }
                }

                e.Graphics.DrawString("Player " + otherPlayerCount, new Font("Segoe UI", 20), Brushes.White, new Point(otherPlayerCardX, otherPlayerCardY + cardHeight));

                foreach (Card c in p.Hand)
                {
                    CardImage = Deck.CardImage(c.Suit, c.Rank);

                    if (CardImage != null)
                    {
                        e.Graphics.DrawImage(CardImage, new Rectangle(otherPlayerCardX, otherPlayerCardY, cardWidth - 20, cardHeight - 20));
                    }

                    if (leftSide) { otherPlayerCardX += (p.Hand.Count * (cardWidth)) / 2; }
                    else { otherPlayerCardX -= (p.Hand.Count * (cardWidth)) / 2; }
                }
                otherPlayerCount--;
            }
        }
    }
}
