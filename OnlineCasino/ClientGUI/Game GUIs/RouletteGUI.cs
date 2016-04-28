using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGUI.Game_GUIs
{
    class RouletteGUI : GameGUI
    {
        Bitmap Wheel = Properties.Resources.roulette_wheel;
        Bitmap Board = Properties.Resources.roulette_numbers;

        public enum Choices
        {
            Number = 0,
            DoubleZero,
            First12,
            Second12,
            Third12,
            First18,
            Even,
            Red,
            Black, 
            Odd,
            Last18,
            TwoToOne1,
            TwoToOne2,
            TwoToOne3,
        };

        public Choices Choice = Choices.Number;
        public int NumberChosen = -1;

        public RouletteGUI(int h, int w)
        {
            clientHeight = h;
            clientWidth = w;
        }

        public void RouletteGUI_Paint(Object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Wheel, 50, clientHeight / 2 - 350 / 2, 350, 350);
            e.Graphics.DrawImage(Board, 415, clientHeight / 2 - 265 / 2, 690, 265);

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

                                    // get win status from server
                                    RES = RoundEndState.Lose;
                                }
                                break;
                            case GameState.Betting:
                                {
                                    
                                }
                                break;
                            case GameState.Playing:
                                {
                                    e.Graphics.FillRectangle(Brushes.White, clientWidth - 150, clientHeight - 120, 100, 35);
                                    e.Graphics.DrawRectangle(Pens.Black, clientWidth - 151, clientHeight - 121, 102, 37);
                                    e.Graphics.DrawString("finish", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));

                                    if (hoverX < clientWidth - 50 && hoverX > clientWidth - 150)
                                    {
                                        if ((hoverY < clientHeight - 120 + 35 && hoverY > clientHeight - 120))
                                        {
                                            e.Graphics.FillRectangle(Brushes.DimGray, clientWidth - 150, clientHeight - 120, 100, 35);
                                            e.Graphics.DrawString("finish", new Font("Segoe UI", 20), Brushes.Black, new Point(clientWidth - 128, clientHeight - 125));
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

                        e.Graphics.DrawString("Play Again", new Font("Segoe UI", 14), Brushes.DarkGray, new Point(clientWidth / 2 - 50, clientHeight / 2 + 65));

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

        public void PaintChosen(Object sender, PaintEventArgs e)
        {

        }

        public int ChosenNumber(int x, int y)
        {
            int num = 0;

            switch (x)
            {
                case 2:
                    {
                        switch (y)                            
                        {
                            case 1:
                                num = 3;
                                break;
                            case 2:
                                num = 2;
                                break;
                            case 3:
                                num = 1;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.First12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.First18;
                                break;                               
                        }
                    }
                    break;
                case 3:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 6;
                                break;
                            case 2:
                                num = 5;
                                break;
                            case 3:
                                num = 4;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.First12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.First18;
                                break;
                        }
                    }
                    break;
                case 4:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 9;
                                break;
                            case 2:
                                num = 8;
                                break;
                            case 3:
                                num = 7;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.First12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Red;
                                break;
                        }
                    }
                    break;
                case 5:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 12;
                                break;
                            case 2:
                                num = 11;
                                break;
                            case 3:
                                num = 10;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.First12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Red;
                                break;
                        }
                    }
                    break;
                case 6:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 15;
                                break;
                            case 2:
                                num = 14;
                                break;
                            case 3:
                                num = 13;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Second12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Red;
                                break;
                        }
                    }
                    break;
                case 7:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 18;
                                break;
                            case 2:
                                num = 17;
                                break;
                            case 3:
                                num = 16;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Second12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Red;
                                break;
                        }
                    }
                    break;
                case 8:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 21;
                                break;
                            case 2:
                                num = 20;
                                break;
                            case 3:
                                num = 19;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Second12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Black;
                                break;
                        }
                    }
                    break;
                case 9:
                    {
                        switch (y)
                        {
                           case 1:
                                num = 24;
                                break;
                            case 2:
                                num = 23;
                                break;
                            case 3:
                                num = 22;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Second12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Black;
                                break;
                        }
                    }
                    break;
                case 10:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 27;
                                break;
                            case 2:
                                num = 26;
                                break;
                            case 3:
                                num = 25;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Third12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Odd;
                                break;
                        }
                    }
                    break;
                case 11:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 30;
                                break;
                            case 2:
                                num = 29;
                                break;
                            case 3:
                                num = 28;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Third12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Odd;
                                break;
                        }
                    }
                    break;
                case 12:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 33;
                                break;
                            case 2:
                                num = 32;
                                break;
                            case 3:
                                num = 31;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Third12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Last18;
                                break;
                        }
                    }
                    break;
                case 13:
                    {
                        switch (y)
                        {
                            case 1:
                                num = 36;
                                break;
                            case 2:
                                num = 35;
                                break;
                            case 3:
                                num = 34;
                                break;
                            case 4:
                                num = -1;
                                Choice = Choices.Third12;
                                break;
                            case 5:
                                num = -1;
                                Choice = Choices.Last18;
                                break;
                        }
                    }
                    break;
                case 14:
                    {
                        switch (y)
                        {
                            case 1:
                                num = -1;
                                break;
                            case 2:
                                num = -1;
                                break;
                            case 3:
                                num = -1;
                                break;
                        }
                    }
                    break;
            }

            NumberChosen = num;
            return num;
        }
    }
}

