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
        Deck Deck = new Deck();
        List<Card> YourHand = new List<Card>();
        List<BlackjackPlayer> OtherPlayers = new List<BlackjackPlayer>();

        Bitmap CardImage;
        Card c = new Card(CardSuit.Clubs, CardRank.Ace);
        Card d = new Card(CardSuit.Diamonds, CardRank.King);
        Card e = new Card(CardSuit.Hearts, CardRank.Eight);

        System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

        private int clientHeight;
        private int clientWidth;

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

        public BlackjackGUI(int h, int w)
        {
            clientHeight = h;
            clientWidth = w;

            YourHand.Add(c);
            YourHand.Add(d);
            YourHand.Add(e);


            yourCardX =  clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = YourHand.Count - 1;

            otherCardX = clientWidth + 20;
            otherCardY = clientHeight - (clientHeight / 2); 
                          
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

                        yourCardOffset = (YourHand.Count * (cardWidth + 20)) / 2;
                        e.Graphics.DrawLine(Pens.Black, new Point(0, clientHeight - cardHeight - 60), new Point(1500, clientHeight - cardHeight - 60));
                        foreach (Card c in YourHand)
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
                        yourCardsCount = YourHand.Count - 1;
                       
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
                                    e.Graphics.DrawString("YOUR TURN", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 145, clientHeight / 2 - 40));
                                }
                                break;
                        } 
                    }
                    break;
                case OverallState.Distributing:
                    {
                        switch(EndStatus)
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
