using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic.Players;
using SharedModels.GameComponents;
using SM = SharedModels.Players;
using ClientLogic;

namespace ClientGUI.Game_GUIs
{
    public class BlackjackGUI : CardGameGUI
    {
        public enum Hand_State
        {
            under21,
            twentyone,
            bust
        }
        public Hand_State HandState = BlackjackGUI.Hand_State.under21;

        SM.User u;
        SM.User v;       
        SM.User x;
        SM.User y;
        SM.User z;

        public new ClientLogic.Players.BlackjackPlayer You;

        public new List<CardPlayer> OtherPlayers;
        public List<Card> DealerHand = new List<Card>();

        protected int dealerCardX;
        protected int dealerCardY = 10;
        protected int dealerCardsCount = 0;
        protected int dealerCardOffset;

        public BlackjackGUI(int h, int w)
        {
            u = new SM.User(100, "n", "nadine", "omg", 100);

            SharedModels.Players.BlackjackPlayer ba = new SharedModels.Players.BlackjackPlayer(u, 1, 100, 100, 100);

            v = new SM.User(100, "f", "Foster", "omg", 100);
            SharedModels.Players.BlackjackPlayer bb = new SharedModels.Players.BlackjackPlayer(v, 2, 100, 100, 100);

            z = new SM.User(100, "h", "Hayden", "omg", 100);
            SharedModels.Players.BlackjackPlayer bc = new SharedModels.Players.BlackjackPlayer(z, 3, 100, 100, 100);

            x = new SM.User(100, "g", "Sandy", "omg", 100);
            SharedModels.Players.BlackjackPlayer bd = new SharedModels.Players.BlackjackPlayer(x, 4, 100, 100, 100);

            y = new SM.User(100, "s", "Gino", "omg", 100);
            SharedModels.Players.BlackjackPlayer be = new SharedModels.Players.BlackjackPlayer(y, 5, 100, 100, 100);

            SM.Player b = new SM.Player(u, 1, 1000);


            Deck = new Deck();
            You = new ClientLogic.Players.BlackjackPlayer(ba);
            ClientLogic.Players.BlackjackPlayer player2 = new ClientLogic.Players.BlackjackPlayer(bb);
            ClientLogic.Players.BlackjackPlayer player3 = new ClientLogic.Players.BlackjackPlayer(bc);
            ClientLogic.Players.BlackjackPlayer player4 = new ClientLogic.Players.BlackjackPlayer(bd);
            ClientLogic.Players.BlackjackPlayer player5 = new ClientLogic.Players.BlackjackPlayer(be);

            OtherPlayers = new List<ClientLogic.Players.BlackjackPlayer>().ConvertAll(x => (CardPlayer)x);
            OtherPlayers.Add(player2);
            OtherPlayers.Add(player3);
            OtherPlayers.Add(player4);
            OtherPlayers.Add(player5);

            clientHeight = h;
            clientWidth = w;

            // remove
            Card you_c = new Card(CardSuit.Clubs, CardRank.King);
            Card you_d = new Card(CardSuit.Diamonds, CardRank.King);
            Card you_e = new Card(CardSuit.Hearts, CardRank.Eight);
            You.Hand.Add(you_c);
            You.Hand.Add(you_d);

            Card d_f = new Card(CardSuit.Hearts, CardRank.Nine);
            Card d_e = new Card(CardSuit.Hearts, CardRank.Seven);

            DealerHand.Add(d_f);
            DealerHand.Add(d_e);

            Card p1_c = new Card(CardSuit.Diamonds, CardRank.Ace);
            Card p1_d = new Card(CardSuit.Hearts, CardRank.Seven);
            OtherPlayers[0].Hand.Add(p1_c);
            OtherPlayers[0].Hand.Add(p1_d);

            Card p2_c = new Card(CardSuit.Diamonds, CardRank.Two);
            Card p2_d = new Card(CardSuit.Spades, CardRank.Eight);
            OtherPlayers[1].Hand.Add(p2_c);
            OtherPlayers[1].Hand.Add(p2_d);

            Card p3_c = new Card(CardSuit.Clubs, CardRank.Eight);
            Card p3_d = new Card(CardSuit.Spades, CardRank.Three);
            OtherPlayers[2].Hand.Add(p3_c);
            OtherPlayers[2].Hand.Add(p3_d);

            Card p4_c = new Card(CardSuit.Clubs, CardRank.Six);
            Card p4_d = new Card(CardSuit.Hearts, CardRank.Queen);
            OtherPlayers[3].Hand.Add(p4_c);
            OtherPlayers[3].Hand.Add(p4_d);

            yourCardX = clientWidth / 2 - cardWidth / 2;
            yourCardY = clientHeight - 200;
            yourCardsCount = You.Hand.Count - 1;

            otherPlayerCardX = (cardWidth - 20) + 50;
            otherPlayerCardY = 100;

        }

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
                        OtherPlayerHands_Paint(sender, e, OtherPlayers, true);

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

                        switch (GS)
                        {
                            case GameState.Waiting:
                                {
                                    e.Graphics.DrawString("Waiting on other players", new Font("Segoe UI", 16), Brushes.White, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                                    
                                    switch (HandState)
                                    {
                                        case Hand_State.under21:
                                            {

                                            }
                                            break;
                                        case Hand_State.twentyone:
                                            {
                                                e.Graphics.DrawString("Blackjack!", new Font("Segoe UI", 22), Brushes.White, new Point(clientWidth / 2 - 60, clientHeight - cardHeight - 80));
                                            }
                                            break;
                                        case Hand_State.bust:
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
                            case GameState.Betting:
                                {
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
                                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
                                }
                                break;
                            case GameState.Playing:
                                {
                                    switch (HandState)
                                    {
                                        case Hand_State.under21:
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
                                        case Hand_State.twentyone:
                                            {
                                                GS = GameState.Waiting;
                                            }
                                            break;
                                        case Hand_State.bust:
                                            {
                                                GS = GameState.Waiting;
                                            }
                                            break;

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

                        e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), Brushes.Black, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));

                        if (hoverX > clientWidth / 2 - 65 && hoverX < clientWidth / 2 - 65 + 130)
                        {
                            if (hoverY > clientHeight / 2 + 60 && hoverY < clientHeight / 2 + 60 + 40)
                            {
                                e.Graphics.FillRectangle(Brushes.DimGray, clientWidth / 2 - 65, clientHeight / 2 + 60, 130, 40);
                                e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), Brushes.DarkGray, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));
                            }
                        }

                        switch (RES)
                        {
                            case RoundEndState.Lose:
                                {
                                    e.Graphics.DrawString("you lose", new Font("Segoe UI", 38), Brushes.Black, new Point(clientWidth / 2 - 95, clientHeight / 2 - 140));
                                    e.Graphics.DrawString("-$" + bet, new Font("Segoe UI", 15), Brushes.Black, new Point(clientWidth / 2 - 15, clientHeight / 2 - 45));
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
