using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic.Players;
using SharedModels.GameComponents;
using SM = SharedModels.Players;
using SMG = SharedModels.Games;
using ClientLogic;
using ClientLogic.Games;
using System;
using SharedModels.Games.Enums;

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

        public List<Card> DealerHand {
            get
            {
                return CurrentGame.DealerHand;
            }
        }

        protected int dealerCardX;
        protected int dealerCardY;
        protected int dealerCardsCount;
        protected int dealerCardOffset;

        public BlackjackGUI(int h, int w)
        {
            clientHeight = h;
            clientWidth = w;

            dealerCardX = clientWidth / 2 - (cardWidth - 20) / 2;
            dealerCardY = 20;
            dealerCardOffset = (DealerHand.Count * (cardWidth + 20)) / 2;
        }

        public void BlackjackGUI_Paint(object sender, PaintEventArgs e)
        {
            switch (ClientMain.MainGame.GS)
            {
                case SMG.GameStates.Waiting:
                    {                     
                        JoiningTable_Draw(sender, e);                    
                    }
                    break;
                case SMG.GameStates.Playing:
                    {
                        YourHand_Paint(sender, e);
                        OtherPlayerHands_Paint(sender, e, CurrentGame.OtherPlayers, true);

                        dealerCardsCount = DealerHand.Count;
                        e.Graphics.DrawString("Dealer", new Font("Segoe UI", 20), Brushes.White, new Point(clientWidth / 2 - 60, cardHeight + 10));

                        dealerCardsCount = DealerHand.Count - 1;
                        dealerCardOffset = (DealerHand.Count * (cardWidth + 5)) / 2;
                        dealerCardX = clientWidth / 2 - dealerCardOffset;

                        if (DealerHand.Count == 1)
                        {
                            CardImage = Deck.CardImage(DealerHand[0].Suit, DealerHand[0].Rank);
                            dealerCardX += (dealerCardsCount * cardWidth + dealerCardsCount * 10);

                            if (CardImage != null)
                            {
                                e.Graphics.DrawImage(CardImage, new Rectangle(dealerCardX - cardWidth / 2, dealerCardY, cardWidth - 20, cardHeight - 20));
                            }

                            e.Graphics.DrawImage(global::ClientGUI.Properties.Resources.Back, new Rectangle(dealerCardX + cardWidth / 2, dealerCardY, cardWidth - 20, cardHeight - 20));
                        }
                        else
                        {
                            foreach (Card c in DealerHand)
                            {
                                dealerCardX += (dealerCardsCount * cardWidth + dealerCardsCount * 5);
                                CardImage = Deck.CardImage(c.Suit, c.Rank);

                                if (CardImage != null)
                                {
                                    e.Graphics.DrawImage(CardImage, new Rectangle(dealerCardX, dealerCardY, cardWidth - 20, cardHeight - 20));
                                }

                                dealerCardsCount--;
                                dealerCardX = clientWidth / 2 - dealerCardOffset;
                            }
                        }

                        switch (CurrentGame.BlackjackState)
                        {
                            case SMG.BlackjackStates.RoundStart:
                                {
                                              
                                }
                                break;
                            case SMG.BlackjackStates.Betting:
                                {
                                   
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
                                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
                                    e.Graphics.DrawString("Waiting on other players to bet..", ClientGUI.FontSmall, Brushes.Black, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                                    var t = e.Graphics.Transform;
                                    if (sp) sx += .013f;
                                    else sx -= .013f;

                                    if (sx < -.28) sp = true;
                                    if (sx > .18) sp = false;

                                    t.Shear(sx, 0);

                                    e.Graphics.Transform = t;
                                    e.Graphics.DrawString(".", ClientGUI.FontSmaller, Brushes.Black, new Point(clientWidth / 2, clientHeight / 2));
                                }
                                break;
                            case SMG.BlackjackStates.Dealing:
                                {

                                }
                                break;
                            case SMG.BlackjackStates.Playing:
                                {
                                    if (CurrentGame.Turn == You.Seat)
                                    {  

                                        e.Graphics.FillRectangle(Brushes.White, clientWidth - 150, clientHeight - 175, 100, 35);
                                        e.Graphics.DrawRectangle(Pens.Black, clientWidth - 151, clientHeight - 176, 102, 37);
                                        e.Graphics.DrawString("Hit", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));

                                        e.Graphics.FillRectangle(Brushes.White, clientWidth - 150, clientHeight - 120, 100, 35);
                                        e.Graphics.DrawRectangle(Pens.Black, clientWidth - 151, clientHeight - 121, 102, 37);
                                        e.Graphics.DrawString("Stay", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));

                                        

                                        switch (You.HandState)
                                        {
                                            case BlackjackHandStates.Under21:
                                                {
                                                    

                                                    if (hoverX < clientWidth - 50 && hoverX > clientWidth - 150)
                                                    {
                                                        if (hoverY < clientHeight - 175 + 35 && hoverY > clientHeight - 175)
                                                        {
                                                            e.Graphics.FillRectangle(ClientGUI.HoverBrush, clientWidth - 150, clientHeight - 175, 100, 35);
                                                            e.Graphics.DrawString("Hit", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));
                                                        }
                                                        else if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
                                                        {
                                                            e.Graphics.FillRectangle(ClientGUI.HoverBrush, clientWidth - 150, clientHeight - 120, 100, 35);
                                                            e.Graphics.DrawString("Stay", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));
                                                        }
                                                    }

                                                    if (You.Action != BlackjackEvents.PlayerStay)
                                                        e.Graphics.DrawString("Your turn..", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 80), ClientGUI.FormatCentered);
                                                    else
                                                        e.Graphics.DrawString("Stay!", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 80), ClientGUI.FormatCentered);

                                                }
                                                break;
                                            case BlackjackHandStates.TwentyOne:
                                                {
                                                    e.Graphics.FillRectangle(ClientGUI.DisabledBrush, clientWidth - 150, clientHeight - 120, 100, 35);
                                                    e.Graphics.DrawString("Stay", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));
                                                    e.Graphics.FillRectangle(ClientGUI.DisabledBrush, clientWidth - 150, clientHeight - 175, 100, 35);
                                                    e.Graphics.DrawString("Hit", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));

                                                    e.Graphics.DrawString("Blackjack!", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 40), ClientGUI.FormatCentered);

                                                }
                                                break;
                                            case BlackjackHandStates.Bust:
                                                {
                                                    e.Graphics.FillRectangle(ClientGUI.DisabledBrush, clientWidth - 150, clientHeight - 120, 100, 35);
                                                    e.Graphics.DrawString("Stay", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));
                                                    e.Graphics.FillRectangle(ClientGUI.DisabledBrush, clientWidth - 150, clientHeight - 175, 100, 35);
                                                    e.Graphics.DrawString("Hit", ClientGUI.FontMedium, Brushes.Black, new Point(clientWidth - 120, clientHeight - 178));

                                                    e.Graphics.DrawString("Bust!", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 40), ClientGUI.FormatCentered);
                                                }
                                                break;
                                        }
                                    }
                                    else if (CurrentGame.Turn != 0)
                                    {
                                        //Other Players Turn
                                       
                                        e.Graphics.DrawString(CurrentGame.Players[CurrentGame.Turn].User.FullName + "'s Turn...", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 140), ClientGUI.FormatCentered);

                                        switch (CurrentGame.TurnPlayer.HandState)
                                        {
                                            case BlackjackHandStates.Under21:
                                                switch (CurrentGame.TurnPlayer.Action)
                                                {
                                                    case BlackjackEvents.PlayerStay:
                                                        e.Graphics.DrawString("Stay!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                        break;
                                                    case BlackjackEvents.PlayerHit:
                                                        e.Graphics.DrawString("Hit!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                        break;

                                                }
                                                
                                                break;
                                            case BlackjackHandStates.TwentyOne:
                                                e.Graphics.DrawString("Blackjack!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                break;
                                            case BlackjackHandStates.Bust:
                                                e.Graphics.DrawString("Bust!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 30), ClientGUI.FormatCentered);
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        //Dealer Turn
                                        e.Graphics.DrawString("Dealer's Turn...", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 140), ClientGUI.FormatCentered);
                                        switch (CurrentGame.DealerHandState)
                                        {
                                            case BlackjackHandStates.Under21:
                                                switch (CurrentGame.DealerAction)
                                                {
                                                    case BlackjackEvents.PlayerStay:
                                                        e.Graphics.DrawString("Stay!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                        break;
                                                    case BlackjackEvents.PlayerHit:
                                                        e.Graphics.DrawString("Hit!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                        break;

                                                }
                                                break;
                                            case BlackjackHandStates.TwentyOne:
                                                e.Graphics.DrawString("Blackjack!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                break;
                                            case BlackjackHandStates.Bust:
                                                e.Graphics.DrawString("Bust!", ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                                break;

                                        }
                                        

                                    }
                                }
                                break;
                            case SMG.BlackjackStates.Payout:
                                {
                                    switch (You.WinLossState)
                                    {
                                        case SM.WinLossStates.Lose:
                                            {
                                                e.Graphics.DrawString("You Lose", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 140), ClientGUI.FormatCentered);
                                                e.Graphics.DrawString("- $" + Math.Abs(You.Gains), ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                            }
                                            break;
                                        case SM.WinLossStates.Tie:
                                            {
                                                e.Graphics.DrawString("You Tied", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 140), ClientGUI.FormatCentered);
                                            }
                                            break;
                                        case SM.WinLossStates.Win:
                                            {
                                                e.Graphics.DrawString("You Won!", ClientGUI.FontLarge, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 140), ClientGUI.FormatCentered);
                                                e.Graphics.DrawString("+ $" + You.Gains, ClientGUI.FontMedium, Brushes.White, new Point(clientWidth / 2, clientHeight / 2 - 70), ClientGUI.FormatCentered);
                                            }
                                            break;
                                    }

                                }
                                break;
                            case SMG.BlackjackStates.RoundFinish:
                                {
                                    //e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 186, 451, 351);
                                    //e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 185, 450, 350));

                                    //e.Graphics.FillRectangle(Brushes.LightGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                                    //e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 66, clientHeight / 2 + 59, 131, 41);

                                    //e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), Brushes.Black, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));

                                    //if (hoverX > clientWidth / 2 - 65 && hoverX < clientWidth / 2 - 65 + 130)
                                    //{
                                    //    if (hoverY > clientHeight / 2 + 60 && hoverY < clientHeight / 2 + 60 + 40)
                                    //    {
                                    //        e.Graphics.FillRectangle(Brushes.DimGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                                    //        e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), ClientGUI.DisabledBrush, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));
                                    //    }
                                    //}
                                }
                                break;

                        }
                    }
                    break;
                case SMG.GameStates.Finializing:
                    {
                        
                    }
                    break;
            }
        }
    }
}
