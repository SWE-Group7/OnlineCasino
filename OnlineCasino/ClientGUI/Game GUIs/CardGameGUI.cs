﻿using ClientLogic;
using ClientLogic.Games;
using ClientLogic.Players;
using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI.Game_GUIs
{
    public abstract class CardGameGUI : GameGUI
    {
        public Bitmap CardImage;

        protected int cardHeight = 150;
        protected int cardWidth = 120;

        protected int yourCardX;
        protected int yourCardY;
        protected int yourCardsCount = 0;
        protected int yourCardOffset;

        protected int YourBalanceOffsetY;
        protected int YourBalanceOffsetX;
        protected int YourNameOffsetY;
        protected int YourNameOffsetX;

        protected int otherPlayerCardX;
        protected int otherPlayerCardY;
        protected int otherPlayerCardCount = 0;
        protected int otherPlayerCardOffset = 0;
        protected int otherPlayerOffset = 0;
        protected int otherPlayerCount = 0;

        public CardGameGUI(int h, int w)
            :base(h, w)
        {
            
            YourNameOffsetY = clientHeight - 300;
            YourNameOffsetX = 100;

            YourBalanceOffsetX = YourNameOffsetX + 20;
            YourBalanceOffsetY = YourNameOffsetY + 35;

        }



        protected void YourHand_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {           
            e.Graphics.DrawLine(Pens.Black, new Point(0, YourNameOffsetY), new Point(1500, YourNameOffsetY));

            string yourBalance = "Balance\t: $" + You.GameBalance + "\nBet\t: $" + You.Bet;
            e.Graphics.DrawString(You.User.FullName, ClientGUI.FontMedium, Brushes.White, new Point(YourNameOffsetX, YourNameOffsetY));
            e.Graphics.DrawString(yourBalance, ClientGUI.FontSmaller, Brushes.White, new Point(YourBalanceOffsetX, YourBalanceOffsetY));

            if (((Blackjack)ClientMain.MainGame).BlackjackState == SharedModels.Games.BlackjackStates.Payout)
            {
                if (You.Gains > 0)
                {
                    e.Graphics.DrawString("\n\n\t+ $" + Math.Abs(You.Gains), ClientGUI.FontSmaller, Brushes.Gold, new Point(YourBalanceOffsetX, YourBalanceOffsetY));
                }
                else if (You.Gains < 0)
                {
                    e.Graphics.DrawString("\n\n\t- $" + Math.Abs(You.Gains), ClientGUI.FontSmaller, Brushes.Gold, new Point(YourBalanceOffsetX, YourBalanceOffsetY));
                }
            }
            yourCardY = clientHeight - cardHeight - 50;
            yourCardsCount = ((CardPlayer)You).Hand.Count - 1;
            yourCardOffset = (((CardPlayer)You).Hand.Count * (cardWidth + 10)) / 2;
            yourCardX = clientWidth / 2 - yourCardOffset;

            foreach (Card c in ((CardPlayer)You).Hand)
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
            yourCardsCount = ((CardPlayer)You).Hand.Count - 1;
        }

        protected void OtherPlayerHands_Paint(object sender, System.Windows.Forms.PaintEventArgs e, List<CardPlayer> OtherPlayers, bool showCards)
        {
            otherPlayerCardX = (cardWidth - 20) + 50;
            otherPlayerCardY = 100;

            otherPlayerCount = OtherPlayers.Count;
            bool leftSide = true;
            foreach (CardPlayer p in OtherPlayers)
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

                string otherUserBalance = "Balance\t: $" + p.GameBalance + "\nBet\t: $" + p.Bet;
                

                e.Graphics.DrawString(p.User.FullName, ClientGUI.FontMedium, Brushes.White, new Point(otherPlayerCardX, otherPlayerCardY + cardHeight));
                e.Graphics.DrawString(otherUserBalance, ClientGUI.FontSmaller, Brushes.White, new Point(otherPlayerCardX + 25, otherPlayerCardY + cardHeight + 40));

                if (((Blackjack)ClientMain.MainGame).BlackjackState == SharedModels.Games.BlackjackStates.Payout)
                {
                    if (p.Gains > 0)
                    {
                        e.Graphics.DrawString("\t+ $" + Math.Abs(p.Gains), ClientGUI.FontSmaller, Brushes.Gold, new Point(otherPlayerCardX, otherPlayerCardY + cardHeight + 60));
                    }
                    else if (p.Gains < 0)
                    {
                        e.Graphics.DrawString("\t- $" + Math.Abs(p.Gains), ClientGUI.FontSmaller, Brushes.Gold, new Point(otherPlayerCardX, otherPlayerCardY + cardHeight + 60));
                    }
                }

                if (showCards)
                {
                    int i = 0;
                    foreach (Card c in p.Hand) 
                    {
                        CardImage = Deck.CardImage(c.Suit, c.Rank);

                        if (CardImage != null)
                        {
                            e.Graphics.DrawImage(CardImage, new Rectangle(otherPlayerCardX + (i * ((leftSide) ? (cardWidth + 5) : -(cardWidth + 5))), otherPlayerCardY, cardWidth - 20, cardHeight - 20));
                        }

                        //if (leftSide) { otherPlayerCardX += (otherPlayerCardCount * (cardWidth)) / 2; }
                        //else { otherPlayerCardX -= (otherPlayerCardCount * (cardWidth)) / 2; }
                        otherPlayerCardCount--;
                        i++;
                    }
                    otherPlayerCount--;
                    
                }
                else
                {
                    foreach (Card c in p.Hand)
                    {
                        e.Graphics.DrawImage(global::ClientGUI.Properties.Resources.Back, new Rectangle(otherPlayerCardX, otherPlayerCardY, cardWidth - 20, cardHeight - 20));
                        if (leftSide) { otherPlayerCardX += (p.Hand.Count * (cardWidth)) / 2; }
                        else { otherPlayerCardX -= (p.Hand.Count * (cardWidth)) / 2; }
                    }
                     
                   
                
                    otherPlayerCount--;
               }
            }
        }
    }
}
