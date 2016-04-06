using System.Drawing;
using System.Windows.Forms;

namespace ClientGUI
{
    partial class ClientGUI
    {
        public string Username;
        public string FullName;
        public string EmailAddress;
        private string Password;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ClientGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1600, 1000);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ClientGUI";
            this.Text = "Online Casino";
            this.Load += new System.EventHandler(this.ClientGUI_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ClientGUI_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;

        TextBox UsernameTextBox;
        TextBox PasswordTextBox;
        TextBox EmailTextBox;
        TextBox FullNameTextBox;

        public void Login_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            Label LoginLabel = new Label();
            LoginLabel.Size = new Size(320, 30);
            LoginLabel.Location = new Point(Width / 2 - 160, Height / 2 - 70);
            LoginLabel.Font = new Font("Segoe UI", 16);
            LoginLabel.BackColor = Color.Transparent;
            LoginLabel.Text = " Welcome to the Online Casino!";
            Controls.Add(LoginLabel);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(65, 20);
            UsernameLabel.Location = new Point(Width / 2 - 135, Height / 2 - 10);
            UsernameLabel.Font = new Font("Segoe UI", 8);
            UsernameLabel.Text = " Username:";
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);

            UsernameTextBox = new TextBox();
            UsernameTextBox.Size = new Size(200, 20);
            UsernameTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 10);           
            Controls.Add(UsernameTextBox);

            Label PasswordLabel = new Label();
            PasswordLabel.Size = new Size(65, 20);
            PasswordLabel.Location = new Point(Width / 2 - 135, Height / 2 + 12);
            PasswordLabel.BackColor = Color.Transparent;
            PasswordLabel.Font = new Font("Segoe UI", 8);
            PasswordLabel.Text = " Password:";
            Controls.Add(PasswordLabel);

            PasswordTextBox = new TextBox();
            PasswordTextBox.Size = new Size(200, 20);
            PasswordTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 12);
            PasswordTextBox.UseSystemPasswordChar = true;
            Controls.Add(PasswordTextBox);

            Button Submit = new Button();
            Submit.Size = new Size(202, 22);
            Submit.BackColor = Color.White;
            Submit.Location = new Point(Width / 2 - 71, Height / 2 + 34);
            Submit.Text = "Sign In";
            Submit.Click += new System.EventHandler(Submit_Click);
            Controls.Add(Submit);

            Button NewUser = new Button();
            NewUser.Size = new Size(202, 22);
            NewUser.BackColor = Color.White;
            NewUser.Location = new Point(Width / 2 - 71, Height / 2 + 56);
            NewUser.Text = "New User?";
            NewUser.Click += new System.EventHandler(NewUser_Click);
            Controls.Add(NewUser);

            
            //Password = PasswordTextBox.Text;
        }

        public void Register_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            this.BackColor = Color.White;
            Label RegisterLabel = new Label();
            RegisterLabel.Size = new Size(250, 30);
            RegisterLabel.Location = new Point(Width / 2 - 125, Height / 2 - 70);
            RegisterLabel.Font = new Font("Segoe UI", 16);
            RegisterLabel.BackColor = Color.Transparent;
            RegisterLabel.Text = "Register a New Account";
            Controls.Add(RegisterLabel);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(65, 20);
            UsernameLabel.Location = new Point(Width / 2 - 135, Height / 2 - 30);
            UsernameLabel.Font = new Font("Segoe UI", 8);
            UsernameLabel.Text = " Username:";              
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);

            UsernameTextBox = new TextBox();
            UsernameTextBox.Size = new Size(200, 20);
            UsernameTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 30);
            Username = UsernameTextBox.Text;
            Controls.Add(UsernameTextBox);

            Label PasswordLabel = new Label();
            PasswordLabel.Size = new Size(65, 20);
            PasswordLabel.Location = new Point(Width / 2 - 135, Height / 2 - 8);
            PasswordLabel.BackColor = Color.Transparent;
            PasswordLabel.Font = new Font("Segoe UI", 8);
            PasswordLabel.Text = " Password:";
            Controls.Add(PasswordLabel);

            PasswordTextBox = new TextBox();
            PasswordTextBox.Size = new Size(200, 20);
            PasswordTextBox.Location = new Point(Width / 2 - 70, Height / 2 - 8);
            PasswordTextBox.UseSystemPasswordChar = true;
            Password = PasswordTextBox.Text;
            Controls.Add(PasswordTextBox);

            Label EmailLabel = new Label();
            EmailLabel.Size = new Size(65, 20);
            EmailLabel.Location = new Point(Width / 2 - 135, Height / 2 + 14);
            EmailLabel.BackColor = Color.Transparent;
            EmailLabel.Font = new Font("Segoe UI", 8);
            EmailLabel.Text = " Email:";
            Controls.Add(EmailLabel);

            EmailTextBox = new TextBox();
            EmailTextBox.Size = new Size(200, 20);
            EmailTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 14);
            EmailAddress = EmailTextBox.Text;
            Controls.Add(EmailTextBox);

            Label FullNameLabel = new Label();
            FullNameLabel.Size = new Size(65, 20);
            FullNameLabel.Location = new Point(Width / 2 - 135, Height / 2 + 36);
            FullNameLabel.BackColor = Color.Transparent;
            FullNameLabel.Font = new Font("Segoe UI", 8);
            FullNameLabel.Text = " Full Name:";
            Controls.Add(FullNameLabel);

            FullNameTextBox = new TextBox();
            FullNameTextBox.Size = new Size(200, 20);
            FullNameTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 36);
            FullName = FullNameTextBox.Text;
            Controls.Add(FullNameTextBox);

            Button Register = new Button();
            Register.Size = new Size(202, 22);
            Register.BackColor = Color.White;
            Register.Location = new Point(Width / 2 - 71, Height / 2 + 58);
            Register.Text = "Register";
            Register.Click += new System.EventHandler(Register_Click);
            Controls.Add(Register);

            Button ReturnToLogin = new Button();
            ReturnToLogin.Size = new Size(50, 22);
            ReturnToLogin.BackColor = Color.White;
            ReturnToLogin.Location = new Point(Width / 2 - 220, Height / 2 + 100);
            ReturnToLogin.Text = "< Back";
            ReturnToLogin.Click += new System.EventHandler(ReturnToLogin_Click);
            Controls.Add(ReturnToLogin);
        }


        public void Menu_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            this.BackColor = Color.White;

            PictureBox MenuHeader = new PictureBox();
            MenuHeader.BackgroundImage = global::ClientGUI.Properties.Resources.Menu_Header;
            MenuHeader.BackgroundImageLayout = ImageLayout.Stretch;
            MenuHeader.Location = new Point(Width / 2 - 150, Height / 2 - 100);
            MenuHeader.Size = new Size(300, 30);
            Controls.Add(MenuHeader);

            Button Blackjack = new Button();         
            Blackjack.BackgroundImage = global::ClientGUI.Properties.Resources.Menu_Blackjack;
            Blackjack.BackgroundImageLayout = ImageLayout.Stretch;
            Blackjack.Size = new Size(175, 200);
            Blackjack.Location = new Point(Width / 2 - 275, Height / 2 - 50);
            Blackjack.Click += new System.EventHandler(Blackjack_Click);
            Controls.Add(Blackjack);

            Button LogOut = new Button();
            LogOut.Size = new Size(75, 22);
            LogOut.BackColor = Color.White;
            LogOut.Location = new Point(Width / 2 - 295, Height / 2 + 200 - 22 - 5);
            LogOut.Text = "< Log Out";
            LogOut.Font = new Font("Segoe UI", 8);
            LogOut.Click += new System.EventHandler(LogOut_Click);
            Controls.Add(LogOut);

            Button AccountInfo = new Button();
            AccountInfo.Size = new Size(100, 22);
            AccountInfo.BackColor = Color.White;
            AccountInfo.Location = new Point(Width / 2 - 220, Height / 2 + 200 - 22 - 5);
            AccountInfo.Text = "Your Account";
            AccountInfo.Font = new Font("Segoe UI", 8);
            AccountInfo.Click += new System.EventHandler(AccountInfo_Click);
            Controls.Add(AccountInfo);

        }

        public void Game_Draw()
        {
            Button ReturnToMenu = new Button();
            ReturnToMenu.Size = new Size(50, 22);
            ReturnToMenu.BackColor = Color.White;
            ReturnToMenu.Location = new Point(20, Height - 75);
            ReturnToMenu.Text = "< Back";
            ReturnToMenu.Click += new System.EventHandler(ReturnToMenu_Click);
            Controls.Add(ReturnToMenu);

            // Make separate classes for each of the game GUIs
            switch (GameChoice)
            {
                case Game.Blackjack:
                    {
                        
                    }
                    break;
            }
        }
    }
}

