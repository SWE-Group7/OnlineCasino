using System.Drawing;
using System.Windows.Forms;

namespace ClientGUI
{
    partial class ClientGUI
    {
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


        public void Menu_Draw()
        {
            this.Controls.Clear();

            Button Blackjack = new Button();
            Blackjack.Size = new Size(60, 100);
            Blackjack.Location = new Point(Width / 2 - 220, Height / 2 + 50);
      
            Controls.Add(Blackjack);

        }

        public void Game_Draw()
        {
            // Make separate classes for each of the game GUIs
        }
    }
}

