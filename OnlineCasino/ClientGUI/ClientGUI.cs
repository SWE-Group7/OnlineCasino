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

        public int mouseX;
        public int mouseY;

        enum State
        {
            Login = 0,
            Register,
            Menu,
            Game
        }

        enum Game
        {
            None = 0,
            Blackjack,
        }

        State ClientState = State.Login;
        Game GameChoice = Game.None;

        public ClientGUI()
        {
            InitializeComponent();
        }

        private void ClientGUI_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = global::ClientGUI.Properties.Resources.CardsBackground;
            Login_Draw();
        }

        private void ClientGUI_Paint(object sender, PaintEventArgs e)
        {
            switch (ClientState)
            {
                case State.Login:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 226, Height / 2 - 126, 451, 251);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 225, Height / 2 - 125, 450, 250));
                    }
                    break;
                case State.Register:
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 226, Height / 2 - 126, 451, 251);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 225, Height / 2 - 125, 450, 250));
                    }
                    break;
                case State.Menu:
                    {
                        // add welcome text with name and balance
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

            ClientState = State.Menu;
            Menu_Draw();
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

            ClientState = State.Menu;
            Menu_Draw();
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
            BlackjackGUI = new BlackjackGUI(Height, Width);
            this.Controls.Clear();
            this.Invalidate();
            this.BackgroundImage = global::ClientGUI.Properties.Resources.BlackjackBackground;

            ClientState = State.Game;
            GameChoice = Game.Blackjack;

            Game_Draw();
        }
        private void AccountInfo_Click(object sender, EventArgs e)
        {
            // Create account information page

        }

        // IN GAME BUTTONS EVENTS
        private void ReturnToMenu_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = global::ClientGUI.Properties.Resources.CardsBackground;
            this.Controls.Clear();
            this.Invalidate();

            ClientState = State.Menu;
            Menu_Draw();

            GameChoice = Game.None;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void ClientGUI_MouseDown(object sender, MouseEventArgs e)
        {                       
            mouseX = e.X;
            mouseY = e.Y;

            BlackjackGUI.clickX = e.X;
            BlackjackGUI.clickY = e.Y;          
        }

        private void ClientGUI_MouseMove(object sender, MouseEventArgs e)
        {
            if (BlackjackGUI != null)
            {
                BlackjackGUI.hoverX = e.X;
                BlackjackGUI.hoverY = e.Y;
            }
        }
    }
}
