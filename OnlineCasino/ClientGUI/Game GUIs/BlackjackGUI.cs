using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic.Players;
using SharedModels.GameComponents;
using SM = SharedModels.Players;
using ClientLogic;
using ClientLogic.Games;

namespace ClientGUI.Game_GUIs
{
    public class BlackjackGUI : CardGameGUI
    {


        public new Blackjack CurrentGame
        {
            get { return (Blackjack)base.CurrentGame; }
        }
        public new BlackjackPlayer You
        {
            get
            {
                return (BlackjackPlayer)base.You;
            }
        }

        public List<Card> DealerHand = new List<Card>();

        protected int dealerCardX;
        protected int dealerCardY = 10;
        protected int dealerCardsCount = 0;
        protected int dealerCardOffset;

        public BlackjackGUI(int h, int w)
        {
            clientHeight = h;
            clientWidth = w;
        }

        public void BlackjackGUI_Paint(object sender, PaintEventArgs e)
        {
            switch (ClientMain.MainGame.OS)
            {
                case OverallStates.Waiting:
                    {
                        switch (CurrentGame.WS)
                        {
                            case WaitingStates.NoConnection:
                                {
                                    JoiningTable_Draw(sender, e);
                                }
                                break;
                            case WaitingStates.TableFound:
                                {
                                    TableFound_Draw(sender, e);
                                }
                                break;
                        }
                    }
                    break;
                case OverallStates.Playing:
                    {
                        YourHand_Paint(sender, e);
                        OtherPlayerHands_Paint(sender, e, CurrentGame.OtherPlayers, true);

                        e.Graphics.DrawString("Dealer", new Font("Segoe UI", 20), Brushes.White, new Point(clientWidth / 2 - 60, cardHeight + 10));
                        dealerCardOffset = (DealerHand.Count * (cardWidth + 20)) / 2;
                        foreach (Card c in DealerHand)
                        {
                            dealerCardX += (dealerCardsCount * cardWidth + dealerCardsCount * 20);

                            if (dealerCardsCount == 1)
                            {
                                e.Graphics.DrawImage(global::ClientGUI.Properties.Resources.Back, new Rectangle(dealerCardX, dealerCardY, cardWidth - 20, cardHeight - 20));
                            }
                            else
                            {
                                CardImage = Deck.CardImage(c.Suit, c.Rank);
                                
                                if (CardImage != null)
                                {
                                    e.Graphics.DrawImage(CardImage, new Rectangle(dealerCardX, dealerCardY, cardWidth - 20, cardHeight - 20));
                                }
                            }
                            dealerCardsCount--;
                            dealerCardX = clientWidth / 2 - dealerCardOffset;
                        }
                        dealerCardsCount = DealerHand.Count - 1;

                        switch (CurrentGame.GS)
                        {
                            case GameStates.Waiting:
                                {
                                    e.Graphics.DrawString("Waiting on other players", new Font("Segoe UI", 16), Brushes.White, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                                    
                                    switch (CurrentGame.HandState)
                                    {
                                        case BlackjackHandStates.Under21:
                                            {

                                            }
                                            break;
                                        case BlackjackHandStates.TwentyOne:
                                            {
                                                e.Graphics.DrawString("Blackjack!", new Font("Segoe UI", 22), Brushes.White, new Point(clientWidth / 2 - 60, clientHeight - cardHeight - 80));
                                            }
                                            break;
                                        case BlackjackHandStates.Bust:
                                            {
                                                e.Graphics.DrawString("Bust!", new Font("Segoe UI", 22), Brushes.White, new Point(clientWidth / 2 - 40, clientHeight - cardHeight - 120));
                                            }
                                            break;                                          
                                    }

                                    var t = e.Graphics.Transform;
                                    if (sp) sx += .02f;
                                    else sx -= .02f;

                                    if (sx < -.28) sp = true;
                                    if (sx > .18) sp = false;

                                    t.Shear(sx, 0);

                                    e.Graphics.Transform = t;
                                    e.Graphics.DrawString(".", new Font("Segoe UI", 12), Brushes.White, new Point(clientWidth / 2, clientHeight / 2));
                                    // get win status from server 

                                   
                                }
                                break;
                            case GameStates.Betting:
                                {
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
                                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
                                }
                                break;
                            case GameStates.Playing:
                                {
                                    switch (CurrentGame.HandState)
                                    {
                                        case BlackjackHandStates.Under21:
                                            {
                                                e.Graphics.DrawString("your turn", new Font("Segoe UI", 38), Brushes.White, new Point(clientWidth / 2 - 130, clientHeight / 2 - 40));

                                                e.Graphics.FillRectangle(Brushes.White, clientWidth - 150, clientHeight - 175, 100, 35);
                                                e.Graphics.DrawRectangle(Pens.Black, clientWidth - 151, clientHeight - 176, 102, 37);
                                                e.Graphics.DrawString("hit", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));

                                                e.Graphics.FillRectangle(Brushes.White, clientWidth - 150, clientHeight - 120, 100, 35);
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
                                        case BlackjackHandStates.TwentyOne:
                                            {
                                                CurrentGame.GS = GameStates.Waiting;
                                            }
                                            break;
                                        case BlackjackHandStates.Bust:
                                            {
                                                CurrentGame.GS = GameStates.Waiting;
                                            }
                                            break;

                                     }
                                }
                                break;
                        }
                    }
                    break;
                case OverallStates.Distributing:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 186, 451, 351);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 185, 450, 350));

                        e.Graphics.FillRectangle(Brushes.LightGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                        e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 66, clientHeight / 2 + 59, 131, 41);

                        e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), Brushes.Black, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));

                        if (hoverX > clientWidth / 2 - 65 && hoverX < clientWidth / 2 - 65 + 130)
                        {
                            if (hoverY > clientHeight / 2 + 60 && hoverY < clientHeight / 2 + 60 + 40)
                            {
                                e.Graphics.FillRectangle(Brushes.DimGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                                e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), Brushes.DarkGray, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));
                            }
                        }

                        switch (CurrentGame.RES)
                        {
                            case RoundEndStates.Lose:
                                {
                                    e.Graphics.DrawString("you lose", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 95, clientHeight / 2 - 140));
                                    e.Graphics.DrawString("-$" + You.Bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 45));
                                    e.Graphics.DrawString("current buy in: $" + You.BuyIn, new Font("Segoe UI", 13), Brushes.Black, new Point(clientWidth / 2 - 75, clientHeight / 2 - 20));
                                }
                                break;
                            case RoundEndStates.Tie:
                                {
                                    e.Graphics.DrawString("you tied", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 118, clientHeight / 2 - 140));
                                }
                                break;
                            case RoundEndStates.Win:
                                {
                                    e.Graphics.DrawString("you won!", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 118, clientHeight / 2 - 140));
                                    e.Graphics.DrawString("+$" + You.Bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 40));
                                    e.Graphics.DrawString("current buy in: $" + You.BuyIn, new Font("Segoe UI", 13), Brushes.Black, new Point(clientWidth / 2 - 75, clientHeight / 2 - 20));
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
