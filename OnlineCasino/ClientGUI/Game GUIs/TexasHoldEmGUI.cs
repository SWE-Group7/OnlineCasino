using ClientGUI.Game_GUIs;
using ClientLogic;
using CLP = ClientLogic.Players;
using SharedModels.GameComponents;
using SharedModels.Players;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGUI.Game_GUIs
{
    public class TexasHoldEmGUI : CardGameGUI
    {
        SharedModels.Players.User u;
        SharedModels.Players.User f;
        public new CLP.TexasHoldEmPlayer You;
        public new List<CLP.CardPlayer> OtherPlayers;
        public List<Card> MiddleHand = new List<Card>();

        int middleCardX;
        int middleCardY;
        int middleCardsCount = 0;
        protected int middleCardOffset;
 

        public TexasHoldEmGUI(int h, int w)
        {
            clientHeight = h;
            clientWidth = w;
            middleCardX = clientWidth / 2 - (cardWidth - 20) / 2;
            middleCardY = 100; 
            
            u = new SharedModels.Players.User(100, "n", "nadine", "omg", 100);

            SharedModels.Players.TexasHoldEmPlayer t = new SharedModels.Players.TexasHoldEmPlayer(u, 1, 100);
            f = new SharedModels.Players.User(200, "f", "foster", "omgomg", 100);
            SharedModels.Players.TexasHoldEmPlayer tf = new SharedModels.Players.TexasHoldEmPlayer(u, 1, 100);

            Deck = new Deck();
            You = new ClientLogic.Players.TexasHoldEmPlayer(t);
            OtherPlayers = new List<ClientLogic.Players.TexasHoldEmPlayer>().ConvertAll(x => (CLP.CardPlayer)x);

            OtherPlayers.Add(new ClientLogic.Players.TexasHoldEmPlayer(tf));
            // Remove
            Card c = new Card(CardSuit.Clubs, CardRank.Ace);
            Card d = new Card(CardSuit.Diamonds, CardRank.King);
            You.Hand.Add(c);
            You.Hand.Add(d);

            OtherPlayers[0].Hand.Add(c);
            OtherPlayers[0].Hand.Add(d);

            MiddleHand.Add(c);
            MiddleHand.Add(c);

            yourCardX = clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = You.Hand.Count - 1;
        }

        public void TexasHoldEmGUI_Paint(object sender, PaintEventArgs e)
        {
            switch (OS)
            {
                case OverallState.Waiting:
                    {
                        switch (WS)
                        {
                            case WaitingState.NoConnection:
                                {
                                    JoiningTable_Draw(sender, e);
                                }
                                break;
                            case WaitingState.TableFound:
                                {
                                    TableFound_Draw(sender, e);
                                }
                                break;
                        }
                    }
                    break;
                case OverallState.Playing:
                    {
                        YourHand_Paint(sender, e, You);

                        OtherPlayerHands_Paint(sender, e, OtherPlayers, false);

                        switch (GS)
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
                            case GameState.Betting:
                                {
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
                                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
                                }
                                break;
                            case GameState.Playing:
                                {
                                    middleCardOffset = (MiddleHand.Count * (cardWidth + 20)) / 2;
                                    foreach (Card c in MiddleHand)
                                    {
                                        CardImage = Deck.CardImage(c.Suit, c.Rank);
                                        middleCardX += (middleCardsCount * cardWidth + middleCardsCount * 20);

                                        if (CardImage != null)
                                        {
                                             e.Graphics.DrawImage(CardImage, new Rectangle(middleCardX, middleCardY, cardWidth - 20, cardHeight - 20));
                                        }

                                        middleCardsCount--;
                                        middleCardX = clientWidth / 2 - middleCardOffset;
                                    }
                                    middleCardsCount = MiddleHand.Count - 1;

                                    e.Graphics.DrawString("your turn", new Font("Segoe UI", 38), Brushes.White, new Point(clientWidth / 2 - 130, clientHeight / 2 - 40));

                                    e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 150, clientHeight - 175, 100, 35);
                                    e.Graphics.DrawString("call", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 125, clientHeight - 175));

                                    e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 150, clientHeight - 120, 100, 35);
                                    e.Graphics.DrawString("raise", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 135, clientHeight - 122));

                                    if (hoverX < clientWidth - 50 && hoverX > clientWidth - 150)
                                    {
                                         if (hoverY < clientHeight - 175 + 35 && hoverY > clientHeight - 175)
                                        {
                                            e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 150, clientHeight - 175, 100, 35);
                                            e.Graphics.DrawString("call", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 125, clientHeight - 175));
                                        }
                                        else if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
                                        {
                                            e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 150, clientHeight - 120, 100, 35);
                                            e.Graphics.DrawString("raise", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 135, clientHeight - 122));
                                        }
                                    }

                                    e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 260, clientHeight - 175, 100, 35);
                                    e.Graphics.DrawString("fold", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 238, clientHeight - 175));

                                    e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 260, clientHeight - 120, 100, 35);
                                    e.Graphics.DrawString("check", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 248, clientHeight - 122));

                                    if (hoverX < clientWidth - 160 && hoverX > clientWidth - 260)
                                    {
                                        if (hoverY < clientHeight - 175 + 35 && hoverY > clientHeight - 175)
                                        {
                                            e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 260, clientHeight - 175, 100, 35);
                                            e.Graphics.DrawString("fold", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 238, clientHeight - 175));
                                        }
                                        else if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
                                        {
                                            e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 260, clientHeight - 120, 100, 35);
                                            e.Graphics.DrawString("check", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 248, clientHeight - 122));
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case OverallState.Distributing:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 186, 451, 351);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 185, 450, 350));

                        e.Graphics.FillRectangle(Brushes.LightGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                        e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 66, clientHeight / 2 + 59, 131, 41);

                        e.Graphics.DrawString("Play again?", new Font("Segoe UI", 14), Brushes.Black, new Point(clientWidth / 2 - 52, clientHeight / 2 + 64));

                        if (hoverX > clientWidth / 2 - 65 && hoverX < clientWidth / 2 - 65 + 130)
                        {
                            if (hoverY > clientHeight / 2 + 60 && hoverY < clientHeight / 2 + 60 + 40)
                            {
                                e.Graphics.FillRectangle(Brushes.DimGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                                e.Graphics.DrawString("Play again?", new Font("Segoe UI", 14), Brushes.DarkGray, new Point(clientWidth / 2 - 52, clientHeight / 2 + 64));
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
                                    e.Graphics.DrawString("+$" + bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 40));
                                    e.Graphics.DrawString("current buy in: $" + buyIn, new Font("Segoe UI", 13), Brushes.Black, new Point(clientWidth / 2 - 75, clientHeight / 2 - 20));
                                }
                                break;
                        }
                    }
                    break;
            }
        }
  
    }
}
