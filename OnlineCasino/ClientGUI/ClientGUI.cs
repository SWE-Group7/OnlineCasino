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

        public ClientGUI()
        {
            InitializeComponent();
        }
       
        enum State
        {
            Login = 0,
            Register,
            Menu,
            Game
        }

        State ClientState = State.Login;

        private void ClientGUI_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = global::ClientGUI.Properties.Resources.CardsBackground;
            Login_Draw();
        }

        private void ClientGUI_Paint(object sender, PaintEventArgs e)
        {
            switch(ClientState)
            {
                case State.Login:
                    //e.Graphics.FillRectangle(Brushes.DarkSlateGray, 0, 0, Width, Height);
                    e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 226, Height / 2 - 176, 451, 251);
                    e.Graphics.FillRectangle(Brushes.Azure, new Rectangle(Width / 2 - 225, Height / 2 - 175, 450, 250));
                    break;
                case State.Register:
                    e.Graphics.FillRectangle(Brushes.Azure, new Rectangle(Width / 2 - 225, Height / 2 - 175, 450, 250));
                    break;
                case State.Menu:

                    break;
                case State.Game:

                    break;
            }
        }

        public void Login_Draw()
        {
            this.Controls.Clear();
            Label LoginLabel = new Label();
            LoginLabel.Size = new Size(310, 20);
            LoginLabel.Location = new Point(Width / 2 - 150, Height / 2 - 120);
            LoginLabel.Font = new Font("Helvetica", 16);
            LoginLabel.BackColor = Color.Transparent;
            LoginLabel.Text = "Welcome to the Online Casino!";
            Controls.Add(LoginLabel);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(65, 20);
            UsernameLabel.Location = new Point(Width / 2 - 135, Height / 2 - 60);
            UsernameLabel.Font = new Font("Helvetica", 8);
            UsernameLabel.Text = " Username:";
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);

            TextBox UsernameTextBox = new TextBox();
            UsernameTextBox.Size = new Size(200, 20);
            UsernameTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 60);
            Controls.Add(UsernameTextBox);

            Label PasswordLabel = new Label();
            PasswordLabel.Size = new Size(65, 20);
            PasswordLabel.Location = new Point(Width / 2 - 135, Height / 2 - 38);
            PasswordLabel.BackColor = Color.Transparent;
            PasswordLabel.Font = new Font("Helvetica", 8);
            PasswordLabel.Text = " Password:";
            Controls.Add(PasswordLabel);

            TextBox PasswordTextBox = new TextBox();
            PasswordTextBox.Size = new Size(200, 20);
            PasswordTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 38);
            PasswordTextBox.UseSystemPasswordChar = true;
            Controls.Add(PasswordTextBox);

            Button Submit = new Button();
            Submit.Size = new Size(202, 22);
            Submit.Location = new Point(Width / 2 - 71, Height / 2 - 16);
            Submit.Text = "Sign In";
            Submit.Click += new System.EventHandler(Submit_Click);
            Controls.Add(Submit);

            Button NewUser = new Button();
            NewUser.Size = new Size(202, 22);
            NewUser.Location = new Point(Width / 2 - 71, Height / 2 + 6);
            NewUser.Text = "New User?";
            NewUser.Click += new System.EventHandler(NewUser_Click);
            Controls.Add(NewUser);
        }
        private void Submit_Click(object sender, EventArgs e)
        {
            // Add log in functionality
            ClientState = State.Menu;
            Menu_Draw();
        }
        private void NewUser_Click(object sender, EventArgs e)
        {
            ClientState = State.Register;
            Register_Draw();
        }

        public void Register_Draw()
        {
            this.Controls.Clear();
            Label RegisterLabel = new Label();
            RegisterLabel.Size = new Size(250, 30);
            RegisterLabel.Location = new Point(Width / 2 - 125, Height / 2 - 120);
            RegisterLabel.Font = new Font("Helvetica", 16);
            RegisterLabel.BackColor = Color.Transparent;
            RegisterLabel.Text = "Register a New Account";
            Controls.Add(RegisterLabel);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(65, 20);
            UsernameLabel.Location = new Point(Width / 2 - 135, Height / 2 - 80);
            UsernameLabel.Font = new Font("Helvetica", 8);
            UsernameLabel.Text = " Username:";
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);

            TextBox UsernameTextBox = new TextBox();
            UsernameTextBox.Size = new Size(200, 20);
            UsernameTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 80);
            Controls.Add(UsernameTextBox);

            Label PasswordLabel = new Label();
            PasswordLabel.Size = new Size(65, 20);
            PasswordLabel.Location = new Point(Width / 2 - 135, Height / 2 - 58);
            PasswordLabel.BackColor = Color.Transparent;
            PasswordLabel.Font = new Font("Helvetica", 8);
            PasswordLabel.Text = " Password:";
            Controls.Add(PasswordLabel);

            TextBox PasswordTextBox = new TextBox();
            PasswordTextBox.Size = new Size(200, 20);
            PasswordTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 58);
            PasswordTextBox.UseSystemPasswordChar = true;
            Controls.Add(PasswordTextBox);

            Label EmailLabel = new Label();
            EmailLabel.Size = new Size(65, 20);
            EmailLabel.Location = new Point(Width / 2 - 135, Height / 2 - 36);
            EmailLabel.BackColor = Color.Transparent;
            EmailLabel.Font = new Font("Helvetica", 8);
            EmailLabel.Text = " Email:";
            Controls.Add(EmailLabel);

            TextBox EmailTextBox = new TextBox();
            EmailTextBox.Size = new Size(200, 20);
            EmailTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 36);
            Controls.Add(EmailTextBox);

            Label FullNameLabel = new Label();
            FullNameLabel.Size = new Size(65, 20);
            FullNameLabel.Location = new Point(Width / 2 - 135, Height / 2 - 14);
            FullNameLabel.BackColor = Color.Transparent;
            FullNameLabel.Font = new Font("Helvetica", 8);
            FullNameLabel.Text = " Full Name:";
            Controls.Add(FullNameLabel);

            TextBox FullNameTextBox = new TextBox();
            FullNameTextBox.Size = new Size(200, 20);
            FullNameTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 14);
            Controls.Add(FullNameTextBox);

            Button Register = new Button();
            Register.Size = new Size(202, 22);
            Register.Location = new Point(Width / 2 - 71, Height / 2 + 8);
            Register.Text = "Register";
            Register.Click += new System.EventHandler(Register_Click);
            Controls.Add(Register);

            Button ReturnToLogin = new Button();
            ReturnToLogin.Size = new Size(50, 22);
            ReturnToLogin.Location = new Point(Width / 2 - 220, Height / 2 + 50);
            ReturnToLogin.Text = "< Back";
            ReturnToLogin.Click += new System.EventHandler(ReturnToLogin_Click);
            Controls.Add(ReturnToLogin);
        }
        private void Register_Click(object sender, EventArgs e)
        {
            // Add account registration functionality
            ClientState = State.Menu;
            Menu_Draw();
        }
        private void ReturnToLogin_Click(object sender, EventArgs e)
        {
            ClientState = State.Login;
            Login_Draw();
        }

        public void Menu_Draw()
        {

        }

        public void Game_Draw()
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
