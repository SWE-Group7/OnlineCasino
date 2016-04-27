using ClientGUI.Game_GUIs;
using ClientLogic;
using ClientLogic.Players;
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

        enum State
        {
            Login = 0,
            Register,
            Menu,
            Betting,
            Game
        }

        enum Game
        {
            None = 0,
            Blackjack,
            TexasHoldEm,
            Roulette
        }

        State ClientState = State.Login;
        Game GameChoice = Game.None;

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
            switch (ClientState)
            {
                case State.Login:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 276, Height / 2 - 176, 551, 351);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 275, Height / 2 - 175, 550, 350));
                    }
                    break;
                case State.Register:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 276, Height / 2 - 176, 551, 351);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 275, Height / 2 - 175, 550, 350));
                    }
                    break;
                case State.Menu:
                    {                        
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 301, Height / 2 - 201, 601, 401);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 300, Height / 2 - 200, 600, 400));
                    }
                    break;
                case State.Betting:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 301, Height / 2 - 201, 601, 401);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 300, Height / 2 - 200, 600, 400));
                    }
                    break;
                case State.Game:
                    {
                        switch (GameChoice)
                        {
                            case Game.Blackjack:
                                {
                                    BlackjackGUI.BlackjackGUI_Paint(sender, e);
                                }
                                break;
                            case Game.TexasHoldEm:
                                {
                                    TexasHoldEmGUI.TexasHoldEmGUI_Paint(sender, e);
                                }
                                break;
                            case Game.Roulette:
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
                ClientState = State.Menu;
                Menu_Draw();
            }
            else
            {
                ErrorLogin_Draw();
            }

        }
        private void NewUser_Click(object sender, EventArgs e)
        {
            ClientState = State.Register;
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
                ClientState = State.Menu;
                Menu_Draw();
            }
            else
            {
                RegisterError_Draw();
            }
        }
        private void ReturnToLogin_Click(object sender, EventArgs e)
        {
            ClientState = State.Login;
            Login_Draw();
        }

        // MENU BUTTONS EVENTS
        private void LogOut_Click(object sender, EventArgs e)
        {
            // Add account log out funcationality
            ClientState = State.Login;
            Login_Draw();
        }
        private void Blackjack_Click(object sender, EventArgs e)
        {
            GameChoice = Game.Blackjack;
            ClientState = State.Betting;
            BuyInScreen_Draw();
        }
        private void TexasHoldEm_Click(object sender, EventArgs e)
        {
            GameChoice = Game.TexasHoldEm;
            ClientState = State.Betting;
            BuyInScreen_Draw();
        }
        private void Roulette_Click(object sender, EventArgs e)
        {
            GameChoice = Game.Roulette;
            ClientState = State.Betting;
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
            if (GameChoice == Game.Blackjack)
            {
                if (BlackjackGUI != null && ClientState != State.Betting && BlackjackGUI.OS != BlackjackGUI.OverallState.Distributing)
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

                    ClientState = State.Menu;
                    Menu_Draw();

                    GameChoice = Game.None;
                }
            }
            else if (GameChoice == Game.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null && ClientState != State.Betting && TexasHoldEmGUI.OS != GameGUI.OverallState.Distributing)
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

                    ClientState = State.Menu;
                    Menu_Draw();

                    GameChoice = Game.None;
                }
            }
            else if (GameChoice == Game.Roulette)
            {
                if (RouletteGUI != null && ClientState != State.Betting && RouletteGUI.OS != GameGUI.OverallState.Distributing)
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

                    ClientState = State.Menu;
                    Menu_Draw();

                    GameChoice = Game.None;
                }
            }
            else
            {
                this.BackgroundImage = global::ClientGUI.Properties.Resources.Possible_Background;
                this.Controls.Clear();
                this.Invalidate();

                ClientState = State.Menu;
                Menu_Draw();

                GameChoice = Game.None;
            }
        }
        private void Yes_Click(object sender, EventArgs e)
        {
            if (GameChoice == Game.Blackjack)
            {
                BlackjackGUI = null;
            }
            else if (GameChoice == Game.TexasHoldEm)
            {
                TexasHoldEmGUI = null;
            }
            else if (GameChoice == Game.Roulette)
            {
                RouletteGUI = null;
            }

            this.BackgroundImage = global::ClientGUI.Properties.Resources.CardsBackground;
            this.Controls.Clear();
            this.Invalidate();

            ClientState = State.Menu;
            Menu_Draw();

            GameChoice = Game.None;

            check = false;

        }
        private void No_Click(object sender, EventArgs e)
        {
            check = false;
            Game_Draw();
        }

        private void SubmitBuyIn_Click(object sender, EventArgs e)
        {
            buyInString = BuyInTextBox.Text;

            if (!decimal.TryParse(buyInString, out buyIn))
            {

            }
            else if (buyIn > ClientMain.MainUser.Balance)
            {

            }
            else
            {
                if (GameChoice == Game.Blackjack)
                {
                    if (BlackjackGUI == null)
                    {
                        BlackjackGUI = new BlackjackGUI(Height, Width);
                        BlackjackGUI.buyIn = buyIn;
                    }

                    this.Controls.Clear();
                    this.Invalidate();
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.BlackjackBackground;

                    ClientState = State.Game;
                    BlackjackGUI.OS = GameGUI.OverallState.Playing;
                    //BlackjackGUI.WS = GameGUI.WaitingState.NoConnection;

                    // Wait for table to be found, then move to betting stage

                    BlackjackGUI.GS = GameGUI.GameState.Betting;
                    BettingScreen_Draw();

                }
                else if (GameChoice == Game.TexasHoldEm)
                {
                    if (TexasHoldEmGUI == null)
                    {
                        TexasHoldEmGUI = new TexasHoldEmGUI(Height, Width);
                        TexasHoldEmGUI.buyIn = buyIn;
                    }

                    this.Controls.Clear();
                    this.Invalidate();
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.BlackjackBackground;

                    ClientState = State.Game;
                    TexasHoldEmGUI.OS = GameGUI.OverallState.Playing;
                    //TexasHoldEmGUI.WS = GameGUI.WaitingState.NoConnection;

                    // Wait for table to be found, then move to betting stage

                    TexasHoldEmGUI.GS = GameGUI.GameState.Betting;

                    BettingScreen_Draw();
                }
                else if (GameChoice == Game.Roulette)
                {
                    if(RouletteGUI == null)
                    {
                        RouletteGUI = new RouletteGUI(Height, Width);
                        RouletteGUI.buyIn = buyIn;
                    }

                    this.Controls.Clear();
                    this.Invalidate();
                    this.BackgroundImage = global::ClientGUI.Properties.Resources.RouletteBackground;

                    ClientState = State.Game;
                    RouletteGUI.OS = GameGUI.OverallState.Playing;

                    // RouletteGUI.WS = GameGUI.WaitingState.NoConnection;

                    // Wait for table to be found, then move to betting stage

                    RouletteGUI.GS = GameGUI.GameState.Betting;
                    Game_Draw();
                }
            }
        }
        private void SubmitBet_Click(object sender, EventArgs e)
        {
            betString = BetTextBox.Text;
            if (!decimal.TryParse(betString, out bet))
            {

            }
            else if (bet > buyIn)
            {

            }
            else
            {
                this.Controls.Clear();
                this.Invalidate();

                if (GameChoice == Game.Blackjack)
                {
                    BlackjackGUI.bet = bet;

                    ClientState = State.Game;
                    BlackjackGUI.OS = GameGUI.OverallState.Playing;
                    BlackjackGUI.GS = GameGUI.GameState.Playing;

                    Game_Draw();
                }
                else if (GameChoice == Game.TexasHoldEm)
                {
                    TexasHoldEmGUI.bet = bet;

                    ClientState = State.Game;
                    TexasHoldEmGUI.OS = GameGUI.OverallState.Playing;
                    TexasHoldEmGUI.GS = GameGUI.GameState.Playing;

                    Game_Draw();
                }
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

            if (GameChoice == Game.Blackjack)
            {
                if (BlackjackGUI != null)
                {
                    BlackjackGUI.clickX = e.X;
                    BlackjackGUI.clickY = e.Y;

                    switch (BlackjackGUI.OS)
                    {
                        case BlackjackGUI.OverallState.Playing:
                            {
                                if (BlackjackGUI.clickX < Width - 50 && BlackjackGUI.clickX > Width - 150)
                                {
                                    if (BlackjackGUI.clickY < Height - 175 + 35 && BlackjackGUI.clickY > Height - 175)
                                    {
                                        if (BlackjackGUI.You.Hand.Count < 5)
                                        {
                                            BlackjackGUI.You.Hand.Add(new SharedModels.GameComponents.Card(SharedModels.GameComponents.CardSuit.Clubs, SharedModels.GameComponents.CardRank.Five));
                                        }
                                    }
                                    else if ((BlackjackGUI.clickY < Height - 120 + 35 && BlackjackGUI.clickY > Height - 120))
                                    {
                                        BlackjackGUI.GS = GameGUI.GameState.Waiting;
                                    }
                                }
                            }
                            break;
                        case BlackjackGUI.OverallState.Waiting:
                            break;
                        case BlackjackGUI.OverallState.Distributing:
                            {
                                if (BlackjackGUI.clickX > Width / 2 - 65 && BlackjackGUI.clickX < Width / 2 - 65 + 130)
                                {
                                    if (BlackjackGUI.clickY > Height / 2 + 60 && BlackjackGUI.clickY < Height / 2 + 60 + 40)
                                    {
                                        BlackjackGUI.You.RefreshHand();
                                        BettingScreen_Draw();

                                        ClientState = State.Betting;
                                        BlackjackGUI.OS = BlackjackGUI.OverallState.Waiting;
                                        BlackjackGUI.GS = BlackjackGUI.GameState.Betting;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            else if (GameChoice == Game.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null)
                {
                    TexasHoldEmGUI.clickX = e.X;
                    TexasHoldEmGUI.clickY = e.Y;

                    switch (TexasHoldEmGUI.OS)
                    {
                        case TexasHoldEmGUI.OverallState.Playing:
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
                        case TexasHoldEmGUI.OverallState.Waiting:
                            break;
                        case TexasHoldEmGUI.OverallState.Distributing:
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
            else if (GameChoice == Game.Roulette)
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
                    switch (RouletteGUI.OS)
                    {
                        case RouletteGUI.OverallState.Playing:
                            {
                                switch (RouletteGUI.GS)
                                {
                                    case RouletteGUI.GameState.Waiting:
                                        {
                                         
                                        }
                                        break;
                                    case RouletteGUI.GameState.Betting:
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
                                    case RouletteGUI.GameState.Playing:
                                        {
                                           
                                        }
                                        break;
                                }
                            }
                            break;
                        case RouletteGUI.OverallState.Waiting:
                            {

                            }
                            break;
                        case RouletteGUI.OverallState.Distributing:
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
            if (GameChoice == Game.Blackjack)
            {
                if (BlackjackGUI != null)
                {
                    BlackjackGUI.hoverX = e.X;
                    BlackjackGUI.hoverY = e.Y;
                }
            }
            else if (GameChoice == Game.TexasHoldEm)
            {
                if (TexasHoldEmGUI != null)
                {
                    TexasHoldEmGUI.hoverX = e.X;
                    TexasHoldEmGUI.hoverY = e.Y;
                }
            }
            else if (GameChoice == Game.Roulette)
            {

            }
        }
    }
}
