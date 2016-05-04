using ClientGUI.Game_GUIs;
using ClientLogic;
using ClientLogic.Games;
using ClientLogic.Players;
using SharedModels.Connection.Enums;
using SharedModels.GameComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGUI
{
    public partial class ClientGUI : Form
    {
        BlackjackGUI BlackjackGUI;
        TexasHoldEmGUI TexasHoldEmGUI;
        Game_GUIs.RouletteGUI RouletteGUI;

        public int mouseX;
        public int mouseY;

        public ClientGUI()
        {
            
            InitializeComponent();
        }

        private void ClientGUI_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
            Login_Draw();
        }

        private void ClientGUI_Paint(object sender, PaintEventArgs e)
        {
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
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 301, Height / 2 - 201, 601, 401);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 300, Height / 2 - 200, 600, 400));
                    }
                    break;
                case ClientStates.Betting:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 301, Height / 2 - 201, 601, 401);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 300, Height / 2 - 200, 600, 400));
                    }
                    break;
                case ClientStates.Game:
                    {
                        switch (ClientMain.GameType)
                        {
                            case GameTypes.Blackjack:
                                {
                                    BlackjackGUI.BlackjackGUI_Paint(sender, e);
                                }
                                break;
                            case GameTypes.TexasHoldEm:
                                {
                                    TexasHoldEmGUI.TexasHoldEmGUI_Paint(sender, e);
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
            // Create account information page
            AccountInfo_Draw();
        }

        // IN GAME BUTTONS EVENTS
        private void ReturnToMenu_Click(object sender, EventArgs e)
        {
            if (ClientMain.GameType == GameTypes.Blackjack)
            {
                if (BlackjackGUI != null && ClientMain.ClientState != ClientStates.Betting && ClientMain.MainGame.OS != OverallStates.Distributing)
                {
                    check = true;
                    Game_Draw();
                }
                else
                {
                    BlackjackGUI = null;
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
                    this.Controls.Clear();
                    this.Invalidate();

                    ClientMain.ClientState = ClientStates.Menu;
                    Menu_Draw();

                    ClientMain.GameType = GameTypes.None;
                }
            }
            else if (ClientMain.GameType == GameTypes.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null && ClientMain.ClientState != ClientStates.Betting && ClientMain.MainGame.OS != OverallStates.Distributing)
                {
                    check = true;
                    Game_Draw();
                }
                else
                {
                    TexasHoldEmGUI = null;
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
                    this.Controls.Clear();
                    this.Invalidate();

                    ClientMain.ClientState = ClientStates.Menu;
                    Menu_Draw();

                    ClientMain.GameType = GameTypes.None;
                }
            }
            else if (ClientMain.GameType == GameTypes.Roulette)
            {
                if (RouletteGUI != null && ClientMain.ClientState != ClientStates.Betting && ClientMain.MainGame.OS != OverallStates.Distributing)
                {
                    check = true;
                    Game_Draw();
                }
                else
                {
                    RouletteGUI = null;
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
                    this.Controls.Clear();
                    this.Invalidate();

                    ClientMain.ClientState = ClientStates.Menu;
                    Menu_Draw();

                    ClientMain.GameType = GameTypes.None;
                }
            }
            else
            {
                this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
                this.Controls.Clear();
                this.Invalidate();

                ClientMain.ClientState = ClientStates.Menu;
                Menu_Draw();

                ClientMain.GameType = GameTypes.None;
            }
        }
        private void Yes_Click(object sender, EventArgs e)
        {
            if (ClientMain.GameType == GameTypes.Blackjack)
            {
                BlackjackGUI = null;
            }
            else if (ClientMain.GameType == GameTypes.TexasHoldEm)
            {
                TexasHoldEmGUI = null;
            }
            else if (ClientMain.GameType == GameTypes.Roulette)
            {
                RouletteGUI = null;
            }

            this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
            this.Controls.Clear();
            this.Invalidate();

            ClientMain.ClientState = ClientStates.Menu;
            Menu_Draw();

            ClientMain.GameType = GameTypes.None;

            check = false;

        }
        private void No_Click(object sender, EventArgs e)
        {
            check = false;
            if (ClientMain.ClientState != ClientStates.Betting) Game_Draw();
            else BettingScreen_Draw();
        }

        private void SubmitBuyIn_Click(object sender, EventArgs e)
        {
            buyInString = BuyInTextBox.Text;

            //Invalid buyIn
            if (!int.TryParse(buyInString, out buyIn) || buyIn > ClientMain.MainUser.Balance)
                return;
            
            ClientMain.BuyIn = buyIn;

            //Try Join
            if (!ClientMain.TryJoinGame(ClientMain.GameType))
                return;
                
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


            ClientMain.MainGame.OS = OverallStates.Playing;
            ClientMain.MainGame.GS = GameStates.Waiting;
            
            
        }
        private void SubmitBet_Click(object sender, EventArgs e)
        {
            betString = BetTextBox.Text;
            if (!int.TryParse(betString, out bet))
            {

            }
            else if (bet > buyIn)
            {

            }
            else
            {
                this.Controls.Clear();
                this.Invalidate();

                
                ClientMain.MainPlayer.Bet = bet;

                ClientMain.ClientState = ClientStates.Game;
                ClientMain.MainGame.OS = OverallStates.Playing;
                ClientMain.MainGame.GS = GameStates.Playing;

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

                    switch (ClientMain.MainGame.OS)
                    {
                        case OverallStates.Playing:
                            {
                                if (BlackjackGUI.clickX < Width - 50 && BlackjackGUI.clickX > Width - 150)
                                {
                                    if (BlackjackGUI.clickY < Height - 175 + 35 && BlackjackGUI.clickY > Height - 175)
                                    {
                                        if (BlackjackGUI.You.Hand.Count < 5)
                                        {
                                            BlackjackGUI.You.Hand.Add(new SharedModels.GameComponents.Card(SharedModels.GameComponents.CardSuit.Clubs, SharedModels.GameComponents.CardRank.Five));
                                        }

                                        ((Blackjack) ClientMain.MainGame).HandState = BlackjackHandStates.Bust;
                                    }
                                    else if ((BlackjackGUI.clickY < Height - 120 + 35 && BlackjackGUI.clickY > Height - 120))
                                    {
                                        ClientMain.MainGame.GS = GameStates.Waiting;
                                    }
                                }

                                if(ClientMain.MainGame.GS == GameStates.Waiting)
                                {
                                    ClientMain.MainGame.OS = OverallStates.Distributing;
                                    ClientMain.MainGame.RES = RoundEndStates.Lose;
                                }
                            }
                            break;
                        case OverallStates.Waiting:
                            {
                                
                            }
                            break;
                        case OverallStates.Distributing:
                            {
                                if (BlackjackGUI.clickX > Width / 2 - 65 && BlackjackGUI.clickX < Width / 2 - 65 + 130)
                                {
                                    if (BlackjackGUI.clickY > Height / 2 + 60 && BlackjackGUI.clickY < Height / 2 + 60 + 40)
                                    {
                                        //ClientMain.MainGame.You.RefreshHand();
                                        BettingScreen_Draw();

                                        ClientMain.ClientState = ClientStates.Betting;
                                        ClientMain.MainGame.OS = OverallStates.Waiting;
                                        ClientMain.MainGame.GS = GameStates.Betting;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            else if (ClientMain.GameType == GameTypes.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null)
                {
                    TexasHoldEmGUI.clickX = e.X;
                    TexasHoldEmGUI.clickY = e.Y;

                    switch (ClientMain.MainGame.OS)
                    {
                        case OverallStates.Playing:
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
                        case OverallStates.Waiting:
                            break;
                        case OverallStates.Distributing:
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
                    switch (ClientMain.MainGame.OS)
                    {
                        case OverallStates.Playing:
                            {
                                switch (ClientMain.MainGame.GS)
                                {
                                    case GameStates.Waiting:
                                        {
                                         
                                        }
                                        break;
                                    case GameStates.Betting:
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
                                    case GameStates.Playing:
                                        {
                                           
                                        }
                                        break;
                                }
                            }
                            break;
                        case OverallStates.Waiting:
                            {

                            }
                            break;
                        case OverallStates.Distributing:
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
