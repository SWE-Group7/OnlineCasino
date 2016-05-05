using ClientGUI.Game_GUIs;
using ClientLogic;
using ClientLogic.Games;
using ClientLogic.Players;
using SharedModels.Connection.Enums;
using SharedModels.GameComponents;
using SharedModels.Games.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGUI
{
    public partial class ClientGUI : Form
    {
        BlackjackGUI BlackjackGUI;
        TexasHoldEmGUI TexasHoldEmGUI;
        Game_GUIs.RouletteGUI RouletteGUI;

        public static SolidBrush HoverBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32("FFAAAAAA", 16)));
        public static SolidBrush DisabledBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32("FF555555", 16)));

        public static StringFormat FormatCentered = new StringFormat();

        public static Font FontLarge = new Font("Segoe UI", 38);
        public static Font FontMediumLarge = new Font("Segoe UI", 30);
        public static Font FontMedium = new Font("Segoe UI", 20);
        public static Font FontSmall = new Font("Segoe UI", 16);
        public static Font FontSmaller = new Font("Segoe UI", 12);

        private List<Tuple<string, long>> DisplayedAlerts;
        private static int AlertSpacerY = 100;
        private static int AlertPositionY = 100;

        public int mouseX;
        public int mouseY;

        private bool DrawBettingScreen = true;
        private bool DrawBackToMenu = true;
        private bool DrawMenu = true;

        public ClientGUI()
        {
            InitializeComponent();
            DisplayedAlerts = new List<Tuple<string, long>>();
            FormatCentered.Alignment = StringAlignment.Center;
            FormatCentered.LineAlignment = StringAlignment.Center;
        }

        private void ClientGUI_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
            Login_Draw();
        }

        private void ClientGUI_Paint(object sender, PaintEventArgs e)
        {
            //Modify model based on server events before drawing anything
            ClientMain.HandleEvents();

           

            switch (ClientMain.ClientState)
            {
                case ClientStates.Login:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 276, Height / 2 - 176, 551, 351);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 275, Height / 2 - 175, 550, 350));
                    }
                    break;
                case ClientStates.Register:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 276, Height / 2 - 176, 551, 351);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 275, Height / 2 - 175, 550, 350));
                    }
                    break;
                case ClientStates.Menu:
                    {        
                        if(DrawMenu)
                        {
                            Menu_Draw();
                            DrawMenu = false;
                        }                    

                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 301, Height / 2 - 201, 601, 401);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 300, Height / 2 - 200, 600, 400));
                    }
                    break;
                case ClientStates.Betting:
                    {
                        //Make sure they've been requested to bet
                        if (ClientMain.ServerRequests[SharedModels.Connection.ClientCommands.Blackjack_GetBet] != -1)
                        {
                            if (!ClientMain.MainPlayer.HasBet())
                            {
                                e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 301, Height / 2 - 201, 601, 401);
                                e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 300, Height / 2 - 200, 600, 400));
                            }

                            //Only draw once per betting session
                            if (DrawBettingScreen)
                            {
                                BettingScreen_Draw();
                                DrawBackToMenu = true;
                                DrawBettingScreen = false;
                            }
                            
                        }
                    }
                    break;
                case ClientStates.Game:
                    {

                        if (DrawBackToMenu)
                        {
                            Game_Draw();
                            DrawBettingScreen = true;
                            DrawBackToMenu = false;
                            DrawMenu = true;
                        }

                        switch (ClientMain.GameType)
                        {
                            case GameTypes.Blackjack:
                                {
                                    BlackjackGUI.BlackjackGUI_Paint(sender, e);
                                }
                                break;
                            case GameTypes.TexasHoldEm:
                                {
                                    //TexasHoldEmGUI.TexasHoldEmGUI_Paint(sender, e);
                                }
                                break;
                            case GameTypes.Roulette:
                                {
                                    RouletteGUI.RouletteGUI_Paint(sender, e);
                                }
                                break;
                        }
                    }
                    break;
            }

            //Handle Alerts
            HandleAlerts(sender, e);
        }

        private void HandleAlerts(object sender, PaintEventArgs e)
        {
            //Get new alerts
            while (ClientMain.AlertMessages.Any())
            {
                Tuple<string, int> alert;
                ClientMain.AlertMessages.TryDequeue(out alert);
                string message = alert.Item1;
                long takeDownTime = alert.Item2 + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

                DisplayedAlerts.Add(new Tuple<string, long>(message, takeDownTime));
                
            }

            //Display alerts and remove old ones
            for(int i = DisplayedAlerts.Count -1; i >= 0; i--)
            {
                string message = DisplayedAlerts[i].Item1;
                long takeDownTime = DisplayedAlerts[i].Item2;

                e.Graphics.DrawString(message, FontMediumLarge, Brushes.White, new Point(ClientSize.Width / 2, AlertPositionY + i * (AlertSpacerY)), FormatCentered);

                if((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) > takeDownTime)
                    DisplayedAlerts.RemoveAt(i);
            }
        }


        // LOGIN PAGE BUTTON EVENTS
        private void Submit_Click(object sender, EventArgs e)
        {
            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Text;

            if (ClientMain.TrySyncLogin(Username, Password))
            {
                ClientMain.ClientState = ClientStates.Menu;
                Menu_Draw();
            }
            else
            {
                ErrorLogin_Draw();
            }

        }
        private void NewUser_Click(object sender, EventArgs e)
        {
            ClientMain.ClientState = ClientStates.Register;
            Register_Draw();
        }

        // REGISTER PAGE BUTTONS EVENTS
        private void Register_Click(object sender, EventArgs e)
        {
            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Text;
            EmailAddress = EmailTextBox.Text;
            FullName = FullNameTextBox.Text;

            if (ClientMain.TrySyncRegister(Username, Password, FullName, EmailAddress))
            {
                ClientMain.ClientState = ClientStates.Menu;
                Menu_Draw();
            }
            else
            {
                RegisterError_Draw();
            }
        }
        private void ReturnToLogin_Click(object sender, EventArgs e)
        {
            ClientMain.ClientState = ClientStates.Login;
            Login_Draw();
        }

        // MENU BUTTONS EVENTS
        private void LogOut_Click(object sender, EventArgs e)
        {
            // Add account log out funcationality
            ClientMain.ClientState = ClientStates.Login;
            Login_Draw();
        }
        private void Blackjack_Click(object sender, EventArgs e)
        {
            ClientMain.GameType = GameTypes.Blackjack;
            BuyInScreen_Draw();
        }
        private void TexasHoldEm_Click(object sender, EventArgs e)
        {
            ClientMain.GameType = GameTypes.TexasHoldEm;
            BuyInScreen_Draw();
        }
        private void Roulette_Click(object sender, EventArgs e)
        {
            ClientMain.GameType = GameTypes.Roulette;
            BuyInScreen_Draw();
        }
        private void AccountInfo_Click(object sender, EventArgs e)
        {
            AccountInfo_Draw();
        }

        // IN GAME BUTTONS EVENTS
        private void ReturnToMenu_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
            ClientMain.ClientState = ClientStates.Menu;
            Menu_Draw();

        }
        private void Cashout_Click(object sender, EventArgs e)
        {
            check = true;
            if (ClientMain.ClientState != ClientStates.Betting)
                Game_Draw();
            else
            {
                BettingScreen_Draw();
            }
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            check = false;
            ClientMain.Alert("You will be cashed out at the end of the round.", 5000);
            ClientMain.SendCommand(SharedModels.Connection.ServerCommands.QuitGame, null);

            if (ClientMain.ClientState != ClientStates.Betting)
                Game_Draw();
            else
            {
                BettingScreen_Draw();
            }
        }
        private void No_Click(object sender, EventArgs e)
        {
            check = false;
            if (ClientMain.ClientState != ClientStates.Betting)
                Game_Draw();
            else BettingScreen_Draw();
        }

        private void SubmitBuyIn_Click(object sender, EventArgs e)
        {
            buyInString = BuyInTextBox.Text;
             
            //Invalid buyIn
            if (!int.TryParse(buyInString, out buyIn) || buyIn > ClientMain.MainUser.Balance)
                return;
            
            ClientMain.BuyIn = buyIn;

            if (!ClientMain.TryJoinGame(ClientMain.GameType))
                return;

            ClientMain.ClientState = ClientStates.Game;

            switch (ClientMain.GameType)
            {
                case GameTypes.Blackjack:
                    BlackjackGUI = new BlackjackGUI(Height, Width);
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.BlackjackBackground;
                    break;
                case GameTypes.TexasHoldEm:
                    TexasHoldEmGUI = new TexasHoldEmGUI(Height, Width);
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.BlackjackBackground;
                    break;
                case GameTypes.Roulette:
                    RouletteGUI = new RouletteGUI(Height, Width);
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.RouletteBackground;
                    break;
                default:
                    break;

            }

            this.Controls.Clear();
            this.Invalidate();
                   
        }
        private void SubmitBet_Click(object sender, EventArgs e)
        {
            betString = BetTextBox.Text;
            if (!int.TryParse(betString, out bet))
            {
                ErrorLabel.Text = "Invalid Input";
                ErrorLabel.Refresh();
            }
            else if (bet > ClientMain.MainPlayer.GameBalance)
            {
                ErrorLabel.Text = "Invalid Amount";
                ErrorLabel.Refresh();
            }
            else
            {
                ErrorLabel.Text = "";
                this.Controls.Clear();
                this.Invalidate();
                              
                ClientMain.MainGame.Bet(bet);
                Game_Draw();               
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void ClientGUI_MouseDown(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            if (ClientMain.GameType == GameTypes.Blackjack)
            {
                if (BlackjackGUI != null)
                {
                    BlackjackGUI.clickX = e.X;
                    BlackjackGUI.clickY = e.Y;

                    if (ClientMain.MainGame == null)
                        return;

                    switch (ClientMain.MainGame.GS)
                    {
                        case SharedModels.Games.GameStates.Playing:
                            
                            switch (((Blackjack)ClientMain.MainGame).BlackjackState)
                            {
                                case SharedModels.Games.BlackjackStates.Playing:
                                    if (BlackjackGUI.clickX < Width - 50 && BlackjackGUI.clickX > Width - 150)
                                    {
                                        if (BlackjackGUI.clickY < Height - 175 + 35 && BlackjackGUI.clickY > Height - 175)
                                        {
                                            if (BlackjackGUI.You.Hand.Count < 5)
                                            {
                                                //Clicked Hit
                                                ((Blackjack)ClientMain.MainGame).Action(BlackjackEvents.PlayerHit);
                                            }
                                        }
                                        else if ((BlackjackGUI.clickY < Height - 120 + 35 && BlackjackGUI.clickY > Height - 120))
                                        {
                                            //Clicked Stay
                                            ((Blackjack)ClientMain.MainGame).Action(BlackjackEvents.PlayerStay);
                                        }
                                    }
                                    break;
                                case SharedModels.Games.BlackjackStates.Betting:
                                    BettingScreen_Draw();
                                    break;
                            }
                                   
                            
                            break;
                        case SharedModels.Games.GameStates.Waiting:
                            {
                                
                            }
                            break;
                        case SharedModels.Games.GameStates.Finializing:
                            {
                                
                            }
                            break;
                    }
                }
            }
            else if (ClientMain.GameType == GameTypes.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null)
                {
                    TexasHoldEmGUI.clickX = e.X;
                    TexasHoldEmGUI.clickY = e.Y;

                    switch (ClientMain.MainGame.GS)
                    {
                        case SharedModels.Games.GameStates.Playing:
                            {
                                if (TexasHoldEmGUI.clickX < Width - 50 && TexasHoldEmGUI.clickX > Width - 150)
                                {
                                    if (TexasHoldEmGUI.clickY < Height - 175 + 35 && TexasHoldEmGUI.clickY > Height - 175)
                                    {
                                        // call
                                    }
                                    else if ((TexasHoldEmGUI.clickY < Height - 120 + 35 && TexasHoldEmGUI.clickY > Height - 120))
                                    {
                                        // raise
                                    }
                                }
                                if (TexasHoldEmGUI.clickX < Width - 160 && TexasHoldEmGUI.clickX > Width - 260)
                                {
                                    if (TexasHoldEmGUI.clickY < Height - 175 + 35 && TexasHoldEmGUI.clickY > Height - 175)
                                    {
                                        // fold
                                    }
                                    else if ((TexasHoldEmGUI.clickY < Height - 120 + 35 && TexasHoldEmGUI.clickY > Height - 120))
                                    {
                                        // check
                                    }
                                }
                            }
                            break;
                        case SharedModels.Games.GameStates.Waiting:
                            break;
                        case SharedModels.Games.GameStates.Finializing:
                            {
                                if (TexasHoldEmGUI.clickX > Width / 2 - 65 && TexasHoldEmGUI.clickX < Width / 2 - 65 + 130)
                                {
                                    if (TexasHoldEmGUI.clickY > Height / 2 + 60 && TexasHoldEmGUI.clickY < Height / 2 + 60 + 40)
                                    {
                                        TexasHoldEmGUI.You.RefreshHand();
                                        BettingScreen_Draw();
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            else if (ClientMain.GameType == GameTypes.Roulette)
            {
                if (RouletteGUI != null)
                {
                    RouletteGUI.clickX = e.X;
                    RouletteGUI.clickY = e.Y;

                    int gridX = 0;
                    int gridY = 0;

                    int startX = 473;
                    int startY = 305;

                    int Chosen = 0;
                    switch (ClientMain.MainGame.GS)
                    {
                        case SharedModels.Games.GameStates.Playing:
                            {                                
                                if (RouletteGUI.clickX >= startX - 36 && RouletteGUI.clickX <= startX) { gridX = 1; }
                                else if (RouletteGUI.clickX > startX && RouletteGUI.clickX <= startX + 49) { gridX = 2; }
                                else if (RouletteGUI.clickX > startX + 49 && RouletteGUI.clickX <= startX + 49 * 2) { gridX = 3; }
                                else if (RouletteGUI.clickX > startX + 49 * 2 && RouletteGUI.clickX <= startX + 49 * 3) { gridX = 4; }
                                else if (RouletteGUI.clickX > startX + 49 * 3 && RouletteGUI.clickX <= startX + 49 * 4) { gridX = 5; }
                                else if (RouletteGUI.clickX > startX + 49 * 4 && RouletteGUI.clickX <= startX + 49 * 5) { gridX = 6; }
                                else if (RouletteGUI.clickX > startX + 49 * 5 && RouletteGUI.clickX <= startX + 49 * 6) { gridX = 7; }
                                else if (RouletteGUI.clickX > startX + 49 * 6 && RouletteGUI.clickX <= startX + 49 * 7) { gridX = 8; }
                                else if (RouletteGUI.clickX > startX + 49 * 7 && RouletteGUI.clickX <= startX + 49 * 8) { gridX = 9; }
                                else if (RouletteGUI.clickX > startX + 49 * 8 && RouletteGUI.clickX <= startX + 49 * 9) { gridX = 10; }
                                else if (RouletteGUI.clickX > startX + 49 * 9 && RouletteGUI.clickX <= startX + 49 * 10) { gridX = 11; }
                                else if (RouletteGUI.clickX > startX + 49 * 10 && RouletteGUI.clickX <= startX + 49 * 11) { gridX = 12; }
                                else if (RouletteGUI.clickX > startX + 49 * 11 && RouletteGUI.clickX <= startX + 49 * 12) { gridX = 13; }
                                else if (RouletteGUI.clickX > startX + 49 * 12 && RouletteGUI.clickX <= startX + 49 * 13) { gridX = 14; }

                                if (gridX != 1)
                                {
                                    if (RouletteGUI.clickY >= startY && RouletteGUI.clickY <= startY + 58)
                                    {
                                        gridY = 1;
                                    }
                                    else if (RouletteGUI.clickY >= startY + 58 && RouletteGUI.clickY < startY + 58 * 2)
                                    {
                                        gridY = 2;
                                    }
                                    else if (RouletteGUI.clickY >= startY + 58 * 2 && RouletteGUI.clickY < startY + 58 * 3)
                                    {
                                        gridY = 3;
                                    }
                                    else if (RouletteGUI.clickY >= startY + 58 * 3 + 36 && RouletteGUI.clickY < startY + 58 * 3 + 36 * 2)
                                    {
                                        gridY = 4;
                                    }
                                    else if (RouletteGUI.clickY >= startY + 58 * 3 + 36 * 2 && RouletteGUI.clickY < startY + 58 * 3 + 36 * 3)
                                    {
                                        gridY = 5;
                                    }

                                    Chosen = RouletteGUI.ChosenNumber(gridX, gridY);

                                }
                                else
                                {
                                    if (RouletteGUI.clickY >= startY && RouletteGUI.clickY <= startY + 88)
                                    {
                                        Chosen = -1;
                                        RouletteGUI.Choice = RouletteGUI.Choices.DoubleZero;
                                    }
                                    else if (RouletteGUI.clickY >= startY + 88 && RouletteGUI.clickY <= startY + 88 * 2)
                                    {
                                        Chosen = 0;
                                        RouletteGUI.NumberChosen = 0;
                                    }
                                }                                                                                                        
                            }
                            break;
                        case SharedModels.Games.GameStates.Waiting:
                            {

                            }
                            break;
                        case SharedModels.Games.GameStates.Finializing:
                            {
                                if (RouletteGUI.clickX > Width / 2 - 65 && RouletteGUI.clickX < Width / 2 - 65 + 130)
                                {
                                    if (RouletteGUI.clickY > Height / 2 + 60 && RouletteGUI.clickY < Height / 2 + 60 + 40)
                                    {
                                        
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }

        private void ClientGUI_MouseMove(object sender, MouseEventArgs e)
        {
            if (ClientMain.GameType == GameTypes.Blackjack)
            {
                if (BlackjackGUI != null)
                {
                    BlackjackGUI.hoverX = e.X;
                    BlackjackGUI.hoverY = e.Y;
                }
            }
            else if (ClientMain.GameType == GameTypes.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null)
                {
                    TexasHoldEmGUI.hoverX = e.X;
                    TexasHoldEmGUI.hoverY = e.Y;
                }
            }
            else if (ClientMain.GameType == GameTypes.Roulette)
            {

            }
        }
    }
}
