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
using ClientLogic.Games;

namespace ClientGUI.Game_GUIs
{
    public class TexasHoldEmGUI : CardGameGUI
    {
        SharedModels.Players.User u;
        SharedModels.Players.User f;
        public new TexasHoldEm CurrentGame
        {
            get { return (TexasHoldEm) base.CurrentGame; }
        }
        public new ClientLogic.Players.TexasHoldEmPlayer You
        {
            get
            {
                return (ClientLogic.Players.TexasHoldEmPlayer) base.You;
            }
        }
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

            yourCardX = clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = You.Hand.Count - 1;
        }

       // public void TexasHoldEmGUI_Paint(object sender, PaintEventArgs e)
        //{
        //    switch (CurrentGame.OS)
        //    {
        //        case OverallStates.Waiting:
        //            {
        //                switch (CurrentGame.WS)
        //                {
        //                    case WaitingStates.NoConnection:
        //                        {
        //                            JoiningTable_Draw(sender, e);
        //                        }
        //                        break;
        //                    case WaitingStates.TableFound:
        //                        {
        //                            TableFound_Draw(sender, e);
        //                        }
        //                        break;
        //                }
        //            }
        //            break;
        //        case OverallStates.Playing:
        //            {
        //                YourHand_Paint(sender, e);

        //                OtherPlayerHands_Paint(sender, e, CurrentGame.OtherPlayers, false);

        //                switch (CurrentGame.GS)
        //                {
        //                    case GameStates.Waiting:
        //                        {
        //                            e.Graphics.DrawString("Waiting on other players", ClientGUI.FontMediumWhiteCenter, Brushes.White, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
        //                            var t = e.Graphics.Transform;

        //                            if (sp) sx += .02f;
        //                            else sx -= .02f;

        //                            if (sx < -.28) sp = true;
        //                            if (sx > .18) sp = false;

        //                            t.Shear(sx, 0);

        //                            e.Graphics.Transform = t;
        //                            e.Graphics.DrawString(".", ClientGUI.FontSmaller, Brushes.White, new Point(clientWidth / 2, clientHeight / 2));

        //                            CurrentGame.OS = OverallStates.Distributing;
        //                            CurrentGame.RES = RoundEndStates.Lose;
        //                        }
        //                        break;
        //                    case GameStates.Betting:
        //                        {
        //                            e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
        //                            e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
        //                        }
        //                        break;
        //                    case GameStates.Playing:
        //                        {
        //                            middleCardOffset = (MiddleHand.Count * (cardWidth + 20)) / 2;
        //                            foreach (Card c in MiddleHand)
        //                            {
        //                                CardImage = Deck.CardImage(c.Suit, c.Rank);
        //                                middleCardX += (middleCardsCount * cardWidth + middleCardsCount * 20);

        //                                if (CardImage != null)
        //                                {
        //                                     e.Graphics.DrawImage(CardImage, new Rectangle(middleCardX, middleCardY, cardWidth - 20, cardHeight - 20));
        //                                }

        //                                middleCardsCount--;
        //                                middleCardX = clientWidth / 2 - middleCardOffset;
        //                            }
        //                            middleCardsCount = MiddleHand.Count - 1;

        //                            e.Graphics.DrawString("your turn", ClientGUI.LargeWhiteCenter, Brushes.White, new Point(clientWidth / 2 - 130, clientHeight / 2 - 40));

        //                            e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 150, clientHeight - 175, 100, 35);
        //                            e.Graphics.DrawString("call", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 125, clientHeight - 175));

        //                            e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 150, clientHeight - 120, 100, 35);
        //                            e.Graphics.DrawString("raise", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 135, clientHeight - 122));

        //                            if (hoverX < clientWidth - 50 && hoverX > clientWidth - 150)
        //                            {
        //                                 if (hoverY < clientHeight - 175 + 35 && hoverY > clientHeight - 175)
        //                                {
        //                                    e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 150, clientHeight - 175, 100, 35);
        //                                    e.Graphics.DrawString("call", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 125, clientHeight - 175));
        //                                }
        //                                else if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
        //                                {
        //                                    e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 150, clientHeight - 120, 100, 35);
        //                                    e.Graphics.DrawString("raise", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 135, clientHeight - 122));
        //                                }
        //                            }

        //                            e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 260, clientHeight - 175, 100, 35);
        //                            e.Graphics.DrawString("fold", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 238, clientHeight - 175));

        //                            e.Graphics.DrawImage(Properties.Resources.UpButton, clientWidth - 260, clientHeight - 120, 100, 35);
        //                            e.Graphics.DrawString("check", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 248, clientHeight - 122));

        //                            if (hoverX < clientWidth - 160 && hoverX > clientWidth - 260)
        //                            {
        //                                if (hoverY < clientHeight - 175 + 35 && hoverY > clientHeight - 175)
        //                                {
        //                                    e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 260, clientHeight - 175, 100, 35);
        //                                    e.Graphics.DrawString("fold", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 238, clientHeight - 175));
        //                                }
        //                                else if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
        //                                {
        //                                    e.Graphics.DrawImage(Properties.Resources.DownButton, clientWidth - 260, clientHeight - 120, 100, 35);
        //                                    e.Graphics.DrawString("check", new Font("Segoe UI", 18), Brushes.Black, new Point(clientWidth - 248, clientHeight - 122));
        //                                }
        //                            }
        //                        }
        //                        break;
        //                }
        //            }
        //            break;
        //        case OverallStates.Distributing:
        //            {
        //                e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 186, 451, 351);
        //                e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 185, 450, 350));

        //                e.Graphics.FillRectangle(Brushes.LightGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
        //                e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 66, clientHeight / 2 + 59, 131, 41);

        //                e.Graphics.DrawString("Play again?", new Font("Segoe UI", 14), Brushes.Black, new Point(clientWidth / 2 - 52, clientHeight / 2 + 64));

        //                if (hoverX > clientWidth / 2 - 65 && hoverX < clientWidth / 2 - 65 + 130)
        //                {
        //                    if (hoverY > clientHeight / 2 + 60 && hoverY < clientHeight / 2 + 60 + 40)
        //                    {
        //                        e.Graphics.FillRectangle(Brushes.DimGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
        //                        e.Graphics.DrawString("Play again?", new Font("Segoe UI", 14), ClientGUI.DisabledBrush, new Point(clientWidth / 2 - 52, clientHeight / 2 + 64));
        //                    }
        //                }

        //                switch (CurrentGame.RES)
        //                {
        //                    case RoundEndStates.Lose:
        //                        {
        //                            e.Graphics.DrawString("you lose", ClientGUI.LargeWhiteCenter, Brushes.Black, new Point(clientWidth / 2 - 95, clientHeight / 2 - 140));
        //                            e.Graphics.DrawString("-$" + You.Bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 40));
        //                            e.Graphics.DrawString("current buy in: $" + You.BuyIn, new Font("Segoe UI", 13), Brushes.Black, new Point(clientWidth / 2 - 75, clientHeight / 2 - 20));

        //                        }
        //                        break;
        //                    case RoundEndStates.Tie:
        //                        {
        //                            e.Graphics.DrawString("you tied", ClientGUI.LargeWhiteCenter, Brushes.Black, new Point(clientWidth / 2 - 118, clientHeight / 2 - 140));
        //                        }
        //                        break;
        //                    case RoundEndStates.Win:
        //                        {
        //                            e.Graphics.DrawString("you won!", ClientGUI.LargeWhiteCenter, Brushes.Black, new Point(clientWidth / 2 - 118, clientHeight / 2 - 140));
        //                            e.Graphics.DrawString("+$" + You.Bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 40));
        //                            e.Graphics.DrawString("current buy in: $" + You.BuyIn, new Font("Segoe UI", 13), Brushes.Black, new Point(clientWidth / 2 - 75, clientHeight / 2 - 20));
        //                        }
        //                        break;
        //                }
        //            }
        //            break;
        //    }
        //}
  
    }
}
