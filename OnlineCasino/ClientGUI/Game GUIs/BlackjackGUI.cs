using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic;
using ClientLogic.Players;
using SharedModels.GameComponents;
using SM = SharedModels.Players;

namespace ClientLogic.Game_GUIs
{
    public class BlackjackGUI : CardGameGUI
    {
        SM.User u;
        public new Players.BlackjackPlayer You;
        public new List<CardPlayer> OtherPlayers;
        public List<Card> DealerHand = new List<Card>();

        // Replace
       
        
        Card c = new Card(CardSuit.Clubs, CardRank.Ace);
        Card d = new Card(CardSuit.Diamonds, CardRank.King);
        Card e = new Card(CardSuit.Hearts, CardRank.Eight);


        public BlackjackGUI(int h, int w)
        {
            u = new SM.User(100, "n", "nadine", "omg", 100);
            SharedModels.Players.BlackjackPlayer b = new SharedModels.Players.BlackjackPlayer(u, 100, 100, 100);
            
            Deck = new Deck();
            You = new Players.BlackjackPlayer(b);
            OtherPlayers = new List<Players.BlackjackPlayer>().ConvertAll(x => (CardPlayer)x);
            clientHeight = h;
            clientWidth = w;

            // Remove
            You.Hand.Add(c);
            You.Hand.Add(d);
            You.Hand.Add(e);

            //// Remove
            //BlackjackPlayer ap = new BlackjackPlayer(new SharedModels.Players.BlackjackPlayer(100, 100));
            //BlackjackPlayer bp = new BlackjackPlayer(new SharedModels.Players.BlackjackPlayer(100, 100));
            //BlackjackPlayer cp = new BlackjackPlayer(new SharedModels.Players.BlackjackPlayer(100, 100));
            //BlackjackPlayer dp = new BlackjackPlayer(new SharedModels.Players.BlackjackPlayer(100, 100));

            //OtherPlayers.Add(ap);
            //OtherPlayers.Add(bp);
            //OtherPlayers.Add(cp);
            //OtherPlayers.Add(dp);

            DealerHand.Add(c);

            //OtherPlayers[0].Hand.Add(c);
            //OtherPlayers[0].Hand.Add(c);

            //OtherPlayers[1].Hand.Add(d);
            //OtherPlayers[1].Hand.Add(e);

            //OtherPlayers[2].Hand.Add(d);
            //OtherPlayers[2].Hand.Add(e);

            //OtherPlayers[3].Hand.Add(d);
            //OtherPlayers[3].Hand.Add(e);

            yourCardX = clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = You.Hand.Count - 1;

            dealerCardX = clientWidth / 2 - (cardWidth - 20) / 2;
            dealerCardY = 20;
            dealerCardOffset = DealerHand.Count - 1;

            otherPlayerCardX = (cardWidth - 20) + 50;
            otherPlayerCardY = 100;
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
                        YourHand_Paint(sender, e, You);

                        OtherPlayerHands_Paint(sender, e, OtherPlayers);

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



    }
}
