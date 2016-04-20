using System.Drawing;
using SharedModels.GameComponents;
using SharedModels.Players;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic;

namespace ClientGUI
{
    public class BlackjackGUI
    {
        public enum OverallState
        {
            Waiting = 0,
            Playing,
            Distributing
        }
        public OverallState OS = OverallState.Waiting;

        public enum WaitingState
        {
            NoConnection = 0,
            TableFound,
            Betting
        }
        public WaitingState WS = WaitingState.NoConnection;

        public enum GameState
        {
            Waiting = 0,
            Playing
        }
        public GameState PS = GameState.Waiting;

        public enum RoundEndState
        {
            Win = 0,
            Lose,
            Tie
        }
        public RoundEndState RES = RoundEndState.Tie;

        Deck Deck = new Deck();
        public BlackjackPlayer You;
        public List<BlackjackPlayer> OtherPlayers = new List<BlackjackPlayer>();
        public List<Card> DealerHand = new List<Card>();

        Bitmap CardImage;

        // Replace
        Card c = new Card(CardSuit.Clubs, CardRank.Ace);
        Card d = new Card(CardSuit.Diamonds, CardRank.King);
        Card e = new Card(CardSuit.Hearts, CardRank.Eight);

        System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

        public decimal buyIn;
        public decimal bet;

        private int clientHeight;
        private int clientWidth;
        public int clickX;
        public int clickY;
        public int hoverX;
        public int hoverY;
        
        int yourCardX;
        int yourCardY;
        int yourCardsCount = 0;
        int yourCardOffset;

        int dealerCardX;
        int dealerCardY;
        int dealerCardsCount = 0;
        int dealerCardOffset;

        int otherCardX;
        int otherCardY;
        int otherCardsCount = 0;
        int otherCardOffset = 0;
        int otherPlayerOffset = 0;
        int otherPlayerCount = 0;

        int cardHeight = 150;
        int cardWidth = 120;      

        public BlackjackGUI(int h, int w)
        {
            You = new BlackjackPlayer(100, 100);
            clientHeight = h;
            clientWidth = w;

            // Remove
            You.Hand.Add(c);
            You.Hand.Add(d);
            You.Hand.Add(e);

            // Remove
            BlackjackPlayer ap = new BlackjackPlayer(1020, 300);
            BlackjackPlayer bp = new BlackjackPlayer(1020, 300);
            BlackjackPlayer cp = new BlackjackPlayer(1020, 300);
            BlackjackPlayer dp = new BlackjackPlayer(1020, 300);

            OtherPlayers.Add(ap);
            OtherPlayers.Add(bp);
            OtherPlayers.Add(cp);
            OtherPlayers.Add(dp);

            DealerHand.Add(c);

            OtherPlayers[0].Hand.Add(c);
            OtherPlayers[0].Hand.Add(c);

            OtherPlayers[1].Hand.Add(d);
            OtherPlayers[1].Hand.Add(e);

            OtherPlayers[2].Hand.Add(d);
            OtherPlayers[2].Hand.Add(e);

            OtherPlayers[3].Hand.Add(d);
            OtherPlayers[3].Hand.Add(e);

            yourCardX = clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = You.Hand.Count - 1;

            dealerCardX = clientWidth / 2 - (cardWidth - 20) / 2;
            dealerCardY = 20;
            dealerCardOffset = DealerHand.Count - 1;

            otherCardX = (cardWidth - 20) + 50;
            otherCardY = 100;
        }

        float sx = 0;
        bool sp = true;

        public void BlackjackGUI_Paint(object sender, PaintEventArgs e)
        {
            switch (OS)
            {
                case OverallState.Waiting:
                    {
                        switch (WS)
                        {
                            case WaitingState.NoConnection:
                                {
                                    Stopwatch.Start();
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
                                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));

                                    e.Graphics.DrawString("JOINING NEXT FREE TABLE", new Font("Segoe UI", 16), Brushes.Black, new Point(clientWidth / 2 - 140, clientHeight / 2 - 30));
                                    var t = e.Graphics.Transform;

                                    if (sp)
                                        sx += .01f;
                                    else
                                        sx -= .01f;
                                    if (sx < -.3) sp = true;
                                    if (sx > .3) sp = false;

                                    t.Shear(sx, 0);

                                    e.Graphics.Transform = t;
                                    e.Graphics.DrawString("......", new Font("Segoe UI", 12), Brushes.Black, new Point(clientWidth / 2, clientHeight / 2 + 10));

                                    CheckConnection();
                                }
                                break;
                            case WaitingState.TableFound:
                                {
                                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
                                    e.Graphics.DrawString("Table found! Seating Players..", new Font("Segoe UI", 16), Brushes.Black, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                                }
                                break;
                        }
                    }
                    break;
                case OverallState.Playing:
                    {
                        //e.Graphics.DrawRectangle(Pens.Black, new Rectangle(clientWidth / 2 - 177, clientHeight / 2 - 52, 352, 102));
                        //e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 175, clientHeight / 2 - 50, 350, 100));
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

                        otherPlayerCount = OtherPlayers.Count;
                        bool leftSide = true;
                        foreach (BlackjackPlayer p in OtherPlayers)
                        {
                            otherCardsCount = p.Hand.Count;
                            
                            if(otherPlayerCount % 2 == 0)
                            {
                                otherCardY = 400;
                                if(otherPlayerCount == 2) { otherCardX = 30; leftSide = true; }
                                else { otherCardX = clientWidth - cardWidth - 30; leftSide = false; }
                            }
                            else
                            {
                                otherCardY = 100;
                                if (otherPlayerCount == 1) { otherCardX = 30; leftSide = true; }
                                else { otherCardX = clientWidth - cardWidth - 30; leftSide = false; }
                            }

                            e.Graphics.DrawString("Player " + otherPlayerCount, new Font("Segoe UI", 20), Brushes.White, new Point(otherCardX, otherCardY + cardHeight)); 

                            foreach (Card c in p.Hand)
                            {
                                CardImage = Deck.CardImage(c.Suit, c.Rank);
                              
                                if (CardImage != null)
                                {
                                    e.Graphics.DrawImage(CardImage, new Rectangle(otherCardX, otherCardY, cardWidth - 20, cardHeight - 20));
                                }

                                if (leftSide) { otherCardX += (p.Hand.Count * (cardWidth)) / 2; }
                                else { otherCardX -= (p.Hand.Count * (cardWidth)) / 2; }
                            }
                            otherPlayerCount--;
                        }

                        e.Graphics.DrawString("Dealer", new Font("Segoe UI", 20), Brushes.White, new Point(clientWidth/2 - 60, cardHeight + 10));
                        dealerCardOffset = (DealerHand.Count * (cardWidth + 20)) / 2;
                        foreach (Card c in DealerHand)
                        {
                            CardImage = Deck.CardImage(c.Suit, c.Rank);
                            dealerCardX += (dealerCardsCount * cardWidth + dealerCardsCount * 20);

                            if (CardImage != null)
                            {
                                e.Graphics.DrawImage(CardImage, new Rectangle(dealerCardX, dealerCardY, cardWidth - 20, cardHeight - 20));
                            }

                            dealerCardsCount--;
                            dealerCardX = clientWidth / 2 - dealerCardOffset;
                        }
                        dealerCardsCount = DealerHand.Count - 1;

                        switch (PS)
                        {

                            case GameState.Waiting:
                                {
                                    e.Graphics.DrawString("Waiting on other players", new Font("Segoe UI", 16), Brushes.White, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                                    var t = e.Graphics.Transform;

                                    if (sp) sx += .02f;
                                    else sx -= .02f;

                                    if (sx < -.28) sp = true;
                                    if (sx > .18) sp = false;

                                    t.Shear(sx, 0);

                                    e.Graphics.Transform = t;
                                    e.Graphics.DrawString(".", new Font("Segoe UI", 12), Brushes.White, new Point(clientWidth / 2, clientHeight / 2));

                                    OS = OverallState.Distributing;
                                    RES = RoundEndState.Lose;
                                }
                                break;
                            case GameState.Playing:
                                {                                  
                                    e.Graphics.DrawString("your turn", new Font("Segoe UI", 38), Brushes.White, new Point(clientWidth / 2 - 130, clientHeight / 2 - 40));

                                    e.Graphics.FillRectangle(Brushes.OldLace, clientWidth - 150, clientHeight - 175, 100, 35);
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth - 151, clientHeight - 176, 102, 37);
                                    e.Graphics.DrawString("hit", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));

                                    e.Graphics.FillRectangle(Brushes.OldLace, clientWidth - 150, clientHeight - 120, 100, 35);
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth - 151, clientHeight - 121, 102, 37);
                                    e.Graphics.DrawString("stay", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));

                                    if (hoverX < clientWidth - 50 && hoverX > clientWidth - 150)
                                    {
                                        if (hoverY < clientHeight - 175 + 35 && hoverY > clientHeight - 175)
                                        {
                                            e.Graphics.FillRectangle(Brushes.DimGray, clientWidth - 150, clientHeight - 175, 100, 35);
                                            e.Graphics.DrawString("hit", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));
                                        }
                                        else if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
                                        {
                                            e.Graphics.FillRectangle(Brushes.DimGray, clientWidth - 150, clientHeight - 120, 100, 35);
                                            e.Graphics.DrawString("stay", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));
                                        }
                                    }                                  
                                }
                                break;
                        }
                    }
                    break;
                case OverallState.Distributing:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 301, clientHeight / 2 - 201, 601, 401);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 300, clientHeight / 2 - 200, 600, 400));

                        e.Graphics.FillRectangle(Brushes.LightGray, clientWidth/2 - 65, clientHeight /2 + 60, 130, 40);
                        e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 66, clientHeight / 2 + 59, 131, 41);

                        e.Graphics.DrawString("Play again?", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth/2 - 65, clientHeight/2 + 62));

                        if (hoverX > clientWidth / 2 - 65 && hoverX < clientWidth / 2 - 65 + 130)
                        {
                            if (hoverY > clientHeight / 2 + 60 && hoverY < clientHeight / 2 + 60 + 40)
                            {
                                e.Graphics.FillRectangle(Brushes.DimGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                                e.Graphics.DrawString("Play again?", new Font("Segoe UI", 18), Brushes.DarkGray, new Point(clientWidth / 2 - 65, clientHeight / 2 + 62));
                            }
                        }

                        switch (RES)
                        {
                            case RoundEndState.Lose:
                                {
                                    e.Graphics.DrawString("you lose", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 95, clientHeight / 2 - 140));
                                    e.Graphics.DrawString("-$" + bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 40));
                                  

                                    e.Graphics.DrawString("current buy in: $" + buyIn, new Font("Segoe UI", 13), Brushes.Black, new Point(clientWidth / 2 - 75, clientHeight / 2 - 20));

                                }
                                break;
                            case RoundEndState.Tie:
                                {
                                    e.Graphics.DrawString("you tied", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 118, clientHeight / 2 - 140));
                                }
                                break;
                            case RoundEndState.Win:
                                {
                                    e.Graphics.DrawString("you won!", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 118, clientHeight / 2 - 140));
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private void CheckConnection()
        {
            if (Stopwatch.ElapsedMilliseconds > 1000)
            {
                OS = OverallState.Playing;
                PS = GameState.Playing;

                Stopwatch.Stop();
                Stopwatch.Reset();
            }
        }

    }
}
