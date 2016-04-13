using System.Drawing;
using SharedModels.GameComponents;
using SharedModels.Players;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic;

namespace ClientGUI
{
    public class BlackjackGUI : Form
    {
        enum OverallState
        {
            Waiting = 0,
            Playing,
            Distributing
        }
        OverallState State = OverallState.Waiting;

        enum WaitingState
        {
            NoConnection = 0,
            TableFound
        }
        WaitingState WaitStatus = WaitingState.NoConnection;

        enum GameState
        {
            Waiting = 0,
            Playing
        }
        GameState PlayStatus = GameState.Waiting;
      
        enum RoundEndState
        {
            Win = 0,
            Lose,
            Tie
        }
        RoundEndState EndStatus = RoundEndState.Tie;

        Deck Deck = new Deck();
        BlackjackPlayer You;
        List<BlackjackPlayer> OtherPlayers = new List<BlackjackPlayer>();

        Bitmap CardImage;

        // Replace
        Card c = new Card(CardSuit.Clubs, CardRank.Ace);
        Card d = new Card(CardSuit.Diamonds, CardRank.King);
        Card e = new Card(CardSuit.Hearts, CardRank.Eight);

        System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

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

        int otherCardX;
        int otherCardY;
        int otherCardsCount = 0;
        int otherCardOffset;

        int cardHeight = 150;
        int cardWidth = 120;      

        public BlackjackGUI(int h, int w)
        {
            You = new BlackjackPlayer(100, 100);
            clientHeight = h;
            clientWidth = w;

            // Replace
            You.Hand.Add(c);
            You.Hand.Add(d);
            You.Hand.Add(e);

            BlackjackPlayer b = new BlackjackPlayer(1020, 300);
            OtherPlayers.Add(b);
            OtherPlayers[0].Hand.Add(c);

            yourCardX = clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = You.Hand.Count - 1;

            otherCardX = clientWidth - cardWidth - 50;
            otherCardY = 50;
        }

        float sx = 0;
        bool sp = true;

        public void BlackjackGUI_Paint(object sender, PaintEventArgs e)
        {
            switch (State)
            {
                case OverallState.Waiting:
                    {
                        switch (WaitStatus)
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
                        e.Graphics.DrawRectangle(Pens.Black, new Rectangle(clientWidth / 2 - 177, clientHeight / 2 - 52, 352, 102));
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 175, clientHeight / 2 - 50, 350, 100));
                        e.Graphics.DrawLine(Pens.Black, new Point(0, clientHeight - cardHeight - 60), new Point(1500, clientHeight - cardHeight - 60));
                     
                        // Draw your hand in center of screen
                        yourCardOffset = (You.Hand.Count * (cardWidth + 20)) / 2;
                        foreach (Card c in You.Hand)
                        {
                            CardImage = Deck.CardImage(c.Suit, c.Rank);
                            yourCardX += (yourCardsCount * cardWidth + yourCardsCount * 20);

                            if (CardImage != null)
                            {
                                e.Graphics.DrawImage(CardImage, new Rectangle(yourCardX, yourCardY, cardWidth, cardHeight));
                            }

                            yourCardsCount -= 1;
                            yourCardX = clientWidth / 2 - yourCardOffset;
                        }
                        yourCardsCount = You.Hand.Count - 1;

                        //Draw all other player hands
                        foreach (BlackjackPlayer p in OtherPlayers)
                        {
                            otherCardsCount = p.Hand.Count;

                            foreach (Card c in p.Hand)
                            {
                                CardImage = Deck.CardImage(c.Suit, c.Rank);

                                if (CardImage != null)
                                {
                                    e.Graphics.DrawImage(CardImage, new Rectangle(otherCardX, otherCardY, cardWidth, cardHeight));
                                }
                            }
                        }

                        switch (PlayStatus)
                        {

                            case GameState.Waiting:
                                {
                                    e.Graphics.DrawString("...", new Font("Segoe UI", 16), Brushes.Black, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                                }
                                break;
                            case GameState.Playing:
                                {                                  
                                    e.Graphics.DrawString("your turn", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 115, clientHeight / 2 - 40));

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

                                    if (clickX < clientWidth - 50 && clickX > clientWidth - 150)
                                    {
                                        if (clickY < clientHeight - 175 + 35 && clickY > clientHeight - 175)
                                        {
                                            // hit
                                        }
                                        else if ((clickY < clientHeight - 120 + 35 && clickY > clientHeight - 120))
                                        {
                                            // stay
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case OverallState.Distributing:
                    {
                        switch (EndStatus)
                        {
                            case RoundEndState.Lose:
                                {

                                }
                                break;
                            case RoundEndState.Tie:
                                {

                                }
                                break;
                            case RoundEndState.Win:
                                {

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
                State = OverallState.Playing;
                PlayStatus = GameState.Playing;

                Stopwatch.Stop();
                Stopwatch.Reset();
            }
        }
    }
}
