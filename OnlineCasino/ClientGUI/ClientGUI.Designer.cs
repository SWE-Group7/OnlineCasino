using ClientLogic;
using SharedModels.Connection.Enums;
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

        public int balance;

        public string buyInString;
        public string betString;

        public int buyIn;
        public int bet;

        private bool check = false;
        private bool leave = false;

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
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClientGUI_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ClientGUI_MouseMove);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;

        TextBox UsernameTextBox;
        TextBox PasswordTextBox;
        TextBox EmailTextBox;
        TextBox FullNameTextBox;

        TextBox BuyInTextBox;
        TextBox BetTextBox;

        Label ErrorLabel;

        public void Login_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            //Label LoginLabel = new Label();
            //LoginLabel.Size = new Size(320, 30);
            //LoginLabel.Location = new Point(Width / 2 - 255, Height / 2 - 165);
            //LoginLabel.Font = ClientGUI.FontMediumWhiteCenter;
            //LoginLabel.BackColor = Color.Transparent;
            //LoginLabel.Text = " Welcome to";
            //Controls.Add(LoginLabel);

            PictureBox Logo = new PictureBox();
            Logo.Image = Properties.Resources.Risiko;
            Logo.SizeMode = PictureBoxSizeMode.StretchImage;
            Logo.Location = new Point(Width / 2 - 137, Height / 2 - 125);
            Logo.Size = new Size(275, 125);
            Controls.Add(Logo);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(81, 20);
            UsernameLabel.Location = new Point(Width / 2 - 115, Height / 2 + 10);
            UsernameLabel.Font = new Font("Segoe UI", 10);
            UsernameLabel.Text = " Username";
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);
            
            UsernameTextBox = new TextBox();
            UsernameTextBox.Size = new Size(140, 18);
            UsernameTextBox.BorderStyle = BorderStyle.None;
            UsernameTextBox.Font = new Font("Segoe UI", 10);
            UsernameTextBox.Location = new Point(Width / 2 - 29, Height / 2 + 10);
            Controls.Add(UsernameTextBox);
            UsernameTextBox.Focus();

            PictureBox tbox1 = new PictureBox();
            tbox1.Image = Properties.Resources.textbox;
            tbox1.Size = new Size(160, 30);            
            tbox1.SizeMode = PictureBoxSizeMode.StretchImage;
            tbox1.Location = new Point(Width / 2 - 35 , Height / 2 + 5);
            tbox1.SendToBack();
            Controls.Add(tbox1);

            Label PasswordLabel = new Label();
            PasswordLabel.Size = new Size(75, 20);
            PasswordLabel.Location = new Point(Width / 2 - 110, Height / 2 + 39);
            PasswordLabel.BackColor = Color.Transparent;
            PasswordLabel.Font = new Font("Segoe UI", 10);
            PasswordLabel.Text = " Password";
            Controls.Add(PasswordLabel);

            PasswordTextBox = new TextBox();
            PasswordTextBox.Size = new Size(140, 18);
            PasswordTextBox.BorderStyle = BorderStyle.None;
            PasswordTextBox.Font = new Font("Segoe UI", 10);
            PasswordTextBox.Location = new Point(Width / 2 - 29, Height / 2 + 41);
            PasswordTextBox.UseSystemPasswordChar = true;
            Controls.Add(PasswordTextBox);

            PictureBox tbox2 = new PictureBox();
            tbox2.Image = Properties.Resources.textbox;
            tbox2.Size = new Size(160, 30);
            tbox2.SizeMode = PictureBoxSizeMode.StretchImage;
            tbox2.Location = new Point(Width / 2 - 35, Height / 2 + 36);
            tbox2.SendToBack();
            Controls.Add(tbox2); 

            Button Submit = new Button();
            Submit.Size = new Size(140, 25);
            Submit.FlatStyle = FlatStyle.Flat;
            Submit.FlatAppearance.BorderSize = 0;
            Submit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Submit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Submit.BackColor = Color.Transparent;
            Submit.Image = Properties.Resources.button;
            Submit.Font = new Font("Segoe UI", 10);
            Submit.Location = new Point(Width / 2 - 25, Height / 2 + 71);
            Submit.Text = "Sign In";
            Submit.Click += new System.EventHandler(Submit_Click);
            Controls.Add(Submit);

            PictureBox butt = new PictureBox();
            butt.Image = Properties.Resources.button;
            butt.Size = new Size(160, 30);
            butt.SizeMode = PictureBoxSizeMode.StretchImage;
            butt.Location = new Point(Width / 2 - 35, Height / 2 + 69);
            butt.SendToBack();
            Controls.Add(butt);

            Button NewUser = new Button();
            NewUser.Size = new Size(140, 25);
            NewUser.BackColor = Color.White;
            NewUser.FlatStyle = FlatStyle.Flat;
            NewUser.FlatAppearance.BorderSize = 0;
            NewUser.FlatAppearance.MouseDownBackColor = Color.Transparent;
            NewUser.FlatAppearance.MouseOverBackColor = Color.Transparent;
            NewUser.Image = Properties.Resources.button;
            NewUser.BackColor = Color.Transparent;
            NewUser.Font = new Font("Segoe UI", 10);
            NewUser.Location = new Point(Width / 2 - 25, Height / 2 + 104);
            NewUser.Text = "New User?";
            NewUser.Click += new System.EventHandler(NewUser_Click);
            Controls.Add(NewUser);

            PictureBox butt2 = new PictureBox();
            butt2.Image = Properties.Resources.button;
            butt2.Size = new Size(160, 30);
            butt2.SizeMode = PictureBoxSizeMode.StretchImage;
            butt2.Location = new Point(Width / 2 - 35, Height / 2 + 102);
            butt2.SendToBack();
            Controls.Add(butt2);

        }

        public void ErrorLogin_Draw()
        {
            Label ErrorLoginLabel = new Label();
            ErrorLoginLabel.Size = new Size(202, 20);
            ErrorLoginLabel.Location = new Point(Width / 2 - 51, Height / 2 + 134);
            ErrorLoginLabel.Font = new Font("Segoe UI", 8);
            ErrorLoginLabel.Text = "Username/Password do not match.";
            ErrorLoginLabel.ForeColor = Color.Red;
            ErrorLoginLabel.BackColor = Color.Transparent;
            Controls.Add(ErrorLoginLabel);
        }

        public void Register_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            PictureBox Logo = new PictureBox();
            Logo.Image = Properties.Resources.Risiko;
            Logo.SizeMode = PictureBoxSizeMode.StretchImage;
            Logo.Location = new Point(Width / 2 - 137, Height / 2 - 125);
            Logo.Size = new Size(275, 125);
            Controls.Add(Logo);

            this.BackColor = Color.White;
            Label RegisterLabel = new Label();
            RegisterLabel.Size = new Size(250, 30);
            RegisterLabel.Location = new Point(Width / 2 - 125, Height / 2 - 5);
            RegisterLabel.Font = ClientGUI.FontSmall;
            RegisterLabel.BackColor = Color.Transparent;
            RegisterLabel.Text = "Register a New Account";
            Controls.Add(RegisterLabel);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(65, 20);
            UsernameLabel.Location = new Point(Width / 2 - 135, Height / 2 + 30);
            UsernameLabel.Font = new Font("Segoe UI", 8);
            UsernameLabel.Text = " Username:";
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);

            UsernameTextBox = new TextBox();
            UsernameTextBox.Size = new Size(200, 20);
            UsernameTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 30);
            Username = UsernameTextBox.Text;
            Controls.Add(UsernameTextBox);

            Label PasswordLabel = new Label();
            PasswordLabel.Size = new Size(65, 20);
            PasswordLabel.Location = new Point(Width / 2 - 135, Height / 2 + 52);
            PasswordLabel.BackColor = Color.Transparent;
            PasswordLabel.Font = new Font("Segoe UI", 8);
            PasswordLabel.Text = " Password:";
            Controls.Add(PasswordLabel);

            PasswordTextBox = new TextBox();
            PasswordTextBox.Size = new Size(200, 20);
            PasswordTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 52);
            PasswordTextBox.UseSystemPasswordChar = true;
            Password = PasswordTextBox.Text;
            Controls.Add(PasswordTextBox);

            Label EmailLabel = new Label();
            EmailLabel.Size = new Size(65, 20);
            EmailLabel.Location = new Point(Width / 2 - 135, Height / 2 + 74);
            EmailLabel.BackColor = Color.Transparent;
            EmailLabel.Font = new Font("Segoe UI", 8);
            EmailLabel.Text = " Email:";
            Controls.Add(EmailLabel);

            EmailTextBox = new TextBox();
            EmailTextBox.Size = new Size(200, 20);
            EmailTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 74);
            EmailAddress = EmailTextBox.Text;
            Controls.Add(EmailTextBox);

            Label FullNameLabel = new Label();
            FullNameLabel.Size = new Size(65, 20);
            FullNameLabel.Location = new Point(Width / 2 - 135, Height / 2 + 96);
            FullNameLabel.BackColor = Color.Transparent;
            FullNameLabel.Font = new Font("Segoe UI", 8);
            FullNameLabel.Text = " Full Name:";
            Controls.Add(FullNameLabel);

            FullNameTextBox = new TextBox();
            FullNameTextBox.Size = new Size(200, 20);
            FullNameTextBox.Location = new Point(Width / 2 - 70, Height / 2 + 96);
            FullName = FullNameTextBox.Text;
            Controls.Add(FullNameTextBox);

            Button Register = new Button();
            Register.Size = new Size(202, 22);
            Register.BackColor = Color.White;
            Register.FlatStyle = FlatStyle.Flat;
            Register.FlatAppearance.BorderSize = 0;
            Register.FlatAppearance.MouseDownBackColor = Color.Transparent;
            Register.FlatAppearance.MouseOverBackColor = Color.Transparent;
            Register.Image = Properties.Resources.button;
            Register.BackColor = Color.Transparent;
            Register.Font = new Font("Segoe UI", 9);
            Register.Location = new Point(Width / 2 - 71, Height / 2 + 118);
            Register.Text = "Register";
            Register.Click += new System.EventHandler(Register_Click);
            Controls.Add(Register);

            PictureBox butt = new PictureBox();
            butt.Image = Properties.Resources.button;
            butt.Size = new Size(202, 22);
            butt.SizeMode = PictureBoxSizeMode.StretchImage;
            butt.Location = new Point(Width / 2 - 71, Height / 2 + 118);
            butt.SendToBack();
            Controls.Add(butt);

            Button ReturnToLogin = new Button();
            ReturnToLogin.Size = new Size(50, 22);
            ReturnToLogin.BackColor = Color.White;
            ReturnToLogin.FlatStyle = FlatStyle.Flat;
            ReturnToLogin.FlatAppearance.BorderSize = 0;
            ReturnToLogin.FlatAppearance.MouseDownBackColor = Color.Transparent;
            ReturnToLogin.FlatAppearance.MouseOverBackColor = Color.Transparent;
            ReturnToLogin.Image = Properties.Resources.button;
            ReturnToLogin.BackColor = Color.Transparent;
            ReturnToLogin.Font = new Font("Segoe UI", 9);
            ReturnToLogin.Location = new Point(Width / 2 - 265, Height / 2 + 145);
            ReturnToLogin.Text = "< Back";
            ReturnToLogin.Click += new System.EventHandler(ReturnToLogin_Click);
            Controls.Add(ReturnToLogin);

            PictureBox butt2 = new PictureBox();
            butt2.Image = Properties.Resources.button;
            butt2.Size = new Size(50, 22);
            butt2.SizeMode = PictureBoxSizeMode.StretchImage;
            butt2.Location = new Point(Width / 2 - 265, Height / 2 + 145);
            butt2.SendToBack();
            Controls.Add(butt2);
        }
        protected void RegisterError_Draw()
        {
            Label ErrorLoginLabel = new Label();
            ErrorLoginLabel.Size = new Size(202, 50);
            ErrorLoginLabel.Location = new Point(Width / 2 - 71, Height / 2 + 120);
            ErrorLoginLabel.Font = new Font("Segoe UI", 8);
            ErrorLoginLabel.Text = "Your account registration was unsuccessful . Please review your form and resubmit.";
            ErrorLoginLabel.ForeColor = Color.Red;
            ErrorLoginLabel.BackColor = Color.Transparent;
            Controls.Add(ErrorLoginLabel);
        }

        public void Menu_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            this.BackColor = Color.White;
            PictureBox Logo = new PictureBox();
            Logo.Image = Properties.Resources.Risiko;
            Logo.SizeMode = PictureBoxSizeMode.StretchImage;
            Logo.Location = new Point(Width / 2 - 87, Height / 2 - 175);
            Logo.Size = new Size(175, 78);
            Controls.Add(Logo);

            Label Welcome = new Label();
            Welcome.Size = new Size(550, 35);
            Welcome.Location = new Point(Width / 2 - 275, Height / 2 - 100);
            Welcome.Font = ClientGUI.FontSmall;
            Welcome.BackColor = Color.White;
            Welcome.Text = "Hi " + ClientMain.MainUser.FullName + "! Your balance is $" + ClientMain.MainUser.Balance.ToString("#.####") + ".";
            Controls.Add(Welcome);

            Button Blackjack = new Button();
            Blackjack.BackgroundImage = global::ClientGUI.Properties.Resources.Menu_Blackjack;
            Blackjack.BackgroundImageLayout = ImageLayout.Stretch;
            Blackjack.Size = new Size(175, 200);
            Blackjack.Location = new Point(Width / 2 - 274, Height / 2 - 65);
            Blackjack.Click += new System.EventHandler(Blackjack_Click);
            Controls.Add(Blackjack);

            Button TexasHoldEm = new Button();
            TexasHoldEm.BackgroundImage = global::ClientGUI.Properties.Resources.Menu_TexasHoldEm;
            TexasHoldEm.BackgroundImageLayout = ImageLayout.Stretch;
            TexasHoldEm.Size = new Size(175, 200);
            TexasHoldEm.Location = new Point(Width / 2 - 87, Height / 2 - 65);
            TexasHoldEm.Click += new System.EventHandler(TexasHoldEm_Click);
            Controls.Add(TexasHoldEm);

            Button Roulette = new Button();
            Roulette.BackgroundImage = global::ClientGUI.Properties.Resources.Menu_Roulette;
            Roulette.BackgroundImageLayout = ImageLayout.Stretch;
            Roulette.Size = new Size(175, 200);
            Roulette.Location = new Point(Width / 2 + 99, Height / 2 - 65);
            Roulette.Click += new System.EventHandler(Roulette_Click);
            Controls.Add(Roulette);

            Button LogOut = new Button();
            LogOut.Size = new Size(75, 22);
            LogOut.FlatStyle = FlatStyle.Flat;
            LogOut.FlatAppearance.BorderSize = 0;
            LogOut.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LogOut.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LogOut.Image = Properties.Resources.button;
            LogOut.BackColor = Color.Transparent;
            LogOut.Font = new Font("Segoe UI", 9);
            LogOut.Location = new Point(Width / 2 - 295, Height / 2 + 200 - 22 - 5);
            LogOut.Text = "log out";
            LogOut.Click += new System.EventHandler(LogOut_Click);
            Controls.Add(LogOut);

            PictureBox butt1 = new PictureBox();
            butt1.Image = Properties.Resources.button;
            butt1.Size = new Size(76, 23);
            butt1.SizeMode = PictureBoxSizeMode.StretchImage;
            butt1.Location = new Point(Width / 2 - 295, Height / 2 + 200 - 22 - 5);
            butt1.SendToBack();
            Controls.Add(butt1);

            Button AccountInfo = new Button();
            AccountInfo.Size = new Size(75, 22);
            AccountInfo.FlatStyle = FlatStyle.Flat;
            AccountInfo.FlatAppearance.BorderSize = 0;
            AccountInfo.FlatAppearance.MouseDownBackColor = Color.Transparent;
            AccountInfo.FlatAppearance.MouseOverBackColor = Color.Transparent;
            AccountInfo.Image = Properties.Resources.button;
            AccountInfo.BackColor = Color.Transparent;
            AccountInfo.Font = new Font("Segoe UI", 9);
            AccountInfo.Location = new Point(Width / 2 - 215, Height / 2 + 200 - 22 - 5);
            AccountInfo.Text = "Account";
            AccountInfo.Click += new System.EventHandler(AccountInfo_Click);
            Controls.Add(AccountInfo);

            PictureBox butt2 = new PictureBox();
            butt2.Image = Properties.Resources.button;
            butt2.Size = new Size(76, 23);
            butt2.SizeMode = PictureBoxSizeMode.StretchImage;
            butt2.Location = new Point(Width / 2 - 215, Height / 2 + 200 - 22 - 5);
            butt2.SendToBack();
            Controls.Add(butt2);

        }

        public void AccountInfo_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            this.BackColor = Color.White;
            PictureBox Logo = new PictureBox();
            Logo.Image = Properties.Resources.Risiko;
            Logo.SizeMode = PictureBoxSizeMode.StretchImage;
            Logo.Location = new Point(Width / 2 - 87, Height / 2 - 175);
            Logo.Size = new Size(175, 78);
            Controls.Add(Logo);

            Label Welcome = new Label();
            Welcome.Size = new Size(550, 35);
            Welcome.Location = new Point(Width / 2 - 275, Height / 2 - 100);
            Welcome.Font = ClientGUI.FontSmall;
            Welcome.BackColor = Color.White;
            Welcome.Text = "Hi " + ClientMain.MainUser.FullName + "! Your balance is $" + ClientMain.MainUser.Balance.ToString("#.####") + ".";
            Controls.Add(Welcome);

            Label UsernameLabel = new Label();
            UsernameLabel.Size = new Size(300, 30);
            UsernameLabel.Location = new Point(Width / 2 - 221, Height / 2);
            UsernameLabel.Font = new Font("Segoe UI", 11);
            UsernameLabel.Text = " Username: " + ClientMain.MainUser.Username;
            UsernameLabel.BackColor = Color.Transparent;
            Controls.Add(UsernameLabel);

            Label EmailLabel = new Label();
            EmailLabel.Size = new Size(300, 30);
            EmailLabel.Location = new Point(Width / 2 - 221, Height / 2 + 30);
            EmailLabel.BackColor = Color.Transparent;
            EmailLabel.Font = new Font("Segoe UI", 11);
            EmailLabel.Text = " Email: " + ClientMain.MainUser.Email;
            Controls.Add(EmailLabel);

            Label FullNameLabel = new Label();
            FullNameLabel.Size = new Size(300, 30);
            FullNameLabel.Location = new Point(Width / 2 - 221, Height / 2 + 60);
            FullNameLabel.BackColor = Color.Transparent;
            FullNameLabel.Font = new Font("Segoe UI", 11);
            FullNameLabel.Text = " Full Name: " + ClientMain.MainUser.FullName;
            Controls.Add(FullNameLabel);

            Button ChangeInfo = new Button();
            ChangeInfo.Size = new Size(100, 30);
            ChangeInfo.FlatStyle = FlatStyle.Flat;
            ChangeInfo.FlatAppearance.BorderSize = 0;
            ChangeInfo.FlatAppearance.MouseDownBackColor = Color.Transparent;
            ChangeInfo.FlatAppearance.MouseOverBackColor = Color.Transparent;
            ChangeInfo.Image = Properties.Resources.button;
            ChangeInfo.BackColor = Color.Transparent;
            ChangeInfo.Font = new Font("Segoe UI", 9);
            ChangeInfo.BackColor = Color.White;
            ChangeInfo.Location = new Point(Width / 2 - 220, Height / 2 + 101);
            ChangeInfo.Text = "Change Account";
            //ChangeInfo.Click += new System.EventHandler();
            Controls.Add(ChangeInfo);

            PictureBox butt = new PictureBox();
            butt.Image = Properties.Resources.button;
            butt.Size = new Size(100, 26);
            butt.SizeMode = PictureBoxSizeMode.StretchImage;
            butt.Location = new Point(Width / 2 - 220, Height / 2 + 100);
            butt.SendToBack();
            Controls.Add(butt);

            Button ReturnToMenu = new Button();
            ReturnToMenu.Size = new Size(75, 22);
            ReturnToMenu.FlatStyle = FlatStyle.Flat;
            ReturnToMenu.FlatAppearance.BorderSize = 0;
            ReturnToMenu.FlatAppearance.MouseDownBackColor = Color.Transparent;
            ReturnToMenu.FlatAppearance.MouseOverBackColor = Color.Transparent;
            ReturnToMenu.Image = Properties.Resources.button;
            ReturnToMenu.BackColor = Color.Transparent;
            ReturnToMenu.Font = new Font("Segoe UI", 9);
            ReturnToMenu.Location = new Point(Width / 2 - 295, Height / 2 + 200 - 22 - 5);
            ReturnToMenu.Text = "< back";
            ReturnToMenu.Click += new System.EventHandler(ReturnToMenu_Click);
            Controls.Add(ReturnToMenu);

            PictureBox butt1 = new PictureBox();
            butt1.Image = Properties.Resources.button;
            butt1.Size = new Size(76, 23);
            butt1.SizeMode = PictureBoxSizeMode.StretchImage;
            butt1.Location = new Point(Width / 2 - 295, Height / 2 + 200 - 22 - 5);
            butt1.SendToBack();
            Controls.Add(butt1);

        }

        public void Game_Draw()
        {
            if (!check)
            {
                this.Controls.Clear();
                this.Invalidate();

                Button ReturnToMenu = new Button();
                ReturnToMenu.Size = new Size(100, 22);
                ReturnToMenu.FlatStyle = FlatStyle.Flat;
                ReturnToMenu.FlatAppearance.BorderSize = 0;
                ReturnToMenu.FlatAppearance.MouseDownBackColor = Color.Transparent;
                ReturnToMenu.FlatAppearance.MouseOverBackColor = Color.Transparent;
                ReturnToMenu.Image = Properties.Resources.button;
                ReturnToMenu.BackColor = Color.Transparent;
                ReturnToMenu.Font = new Font("Segoe UI", 9);
                ReturnToMenu.Location = new Point(20, Height - 75);
                ReturnToMenu.Text = "Cash Out";
                ReturnToMenu.Click += new System.EventHandler(Cashout_Click);
                Controls.Add(ReturnToMenu);

                PictureBox butt1 = new PictureBox();
                butt1.Image = Properties.Resources.button;
                butt1.Size = new Size(55, 22);
                butt1.SizeMode = PictureBoxSizeMode.StretchImage;
                butt1.Location = new Point(20, Height - 75);
                butt1.SendToBack();
                Controls.Add(butt1);
            }
            else
            {
                this.Controls.Clear();
                this.Invalidate();

                Label AreYouSure = new Label();
                AreYouSure.Size = new Size(78, 20);
                AreYouSure.Location = new Point(17, Height - 100);
                AreYouSure.Font = new Font("Segoe UI", 8);
                AreYouSure.Text = "Are you sure?";
                Controls.Add(AreYouSure);

                Button Yes = new Button();
                Yes.Size = new Size(35, 22);
                Yes.BackColor = Color.White;
                Yes.Location = new Point(15, Height - 75);
                Yes.Text = "yes";
                Yes.Click += new System.EventHandler(Yes_Click);
                Controls.Add(Yes);

                Button No = new Button();
                No.Size = new Size(35, 22);
                No.BackColor = Color.White;
                No.Location = new Point(60, Height - 75);
                No.Text = "no";
                No.Click += new System.EventHandler(No_Click);
                Controls.Add(No);
            }
        }

        public void BuyInScreen_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            Button ReturnToMenu = new Button();
            ReturnToMenu.Size = new Size(55, 22);
            ReturnToMenu.FlatStyle = FlatStyle.Flat;
            ReturnToMenu.FlatAppearance.BorderSize = 0;
            ReturnToMenu.FlatAppearance.MouseDownBackColor = Color.Transparent;
            ReturnToMenu.FlatAppearance.MouseOverBackColor = Color.Transparent;
            ReturnToMenu.Image = Properties.Resources.button;
            ReturnToMenu.BackColor = Color.Transparent;
            ReturnToMenu.Font = new Font("Segoe UI", 9);
            ReturnToMenu.Location = new Point(20, Height - 75);
            ReturnToMenu.Text = "Back";
            ReturnToMenu.Click += new System.EventHandler(ReturnToMenu_Click);
            Controls.Add(ReturnToMenu);

            PictureBox butt1 = new PictureBox();
            butt1.Image = Properties.Resources.button;
            butt1.Size = new Size(55, 22);
            butt1.SizeMode = PictureBoxSizeMode.StretchImage;
            butt1.Location = new Point(20, Height - 75);
            butt1.SendToBack();
            Controls.Add(butt1);

            Label BettingLabel = new Label();
            BettingLabel.Size = new Size(170, 60);
            BettingLabel.Location = new Point(Width / 2 - 85, Height / 2 - 100);
            BettingLabel.Font = new Font("Segoe UI", 30);
            BettingLabel.BackColor = Color.Transparent;
            BettingLabel.Text = " BUY IN ";
            Controls.Add(BettingLabel);

            Label BuyInLabel = new Label();
            BuyInLabel.Size = new Size(20, 30);
            BuyInLabel.Location = new Point(Width / 2 - 52, Height / 2 - 15);
            BuyInLabel.Font = ClientGUI.FontSmaller;
            BuyInLabel.Text = "$";
            BuyInLabel.BackColor = Color.Transparent;
            Controls.Add(BuyInLabel);

            BuyInTextBox = new TextBox();
            BuyInTextBox.Size = new Size(70, 30);
            BuyInTextBox.Font = ClientGUI.FontSmaller;
            BuyInTextBox.Location = new Point(Width / 2 - 32, Height / 2 - 15);
            Controls.Add(BuyInTextBox);

            Button SubmitBuyIn = new Button();
            SubmitBuyIn.Size = new Size(100, 30);
            SubmitBuyIn.BackColor = Color.White;
            SubmitBuyIn.Location = new Point(Width / 2 - 50, Height / 2 + 50);
            SubmitBuyIn.Font = new Font("Segoe UI", 10);
            SubmitBuyIn.Text = "Find Table";
            SubmitBuyIn.Click += new System.EventHandler(SubmitBuyIn_Click);
            Controls.Add(SubmitBuyIn);
        }
        public void BuyInError_Draw()
        {
            Label ErrorLoginLabel = new Label();
            ErrorLoginLabel.Size = new Size(202, 20);
            ErrorLoginLabel.Location = new Point(Width / 2 - 71, Height / 2 + 80);
            ErrorLoginLabel.Font = new Font("Segoe UI", 8);
            ErrorLoginLabel.Text = "Username/Password do not match.";
            ErrorLoginLabel.ForeColor = Color.Red;
            ErrorLoginLabel.BackColor = Color.Transparent;
            Controls.Add(ErrorLoginLabel);
        }

        public void BettingScreen_Draw()
        {
            this.Controls.Clear();
            this.Invalidate();

            Button ReturnToMenu = new Button();
            ReturnToMenu.Size = new Size(100, 22);
            ReturnToMenu.FlatStyle = FlatStyle.Flat;
            ReturnToMenu.FlatAppearance.BorderSize = 0;
            ReturnToMenu.FlatAppearance.MouseDownBackColor = Color.Transparent;
            ReturnToMenu.FlatAppearance.MouseOverBackColor = Color.Transparent;
            ReturnToMenu.Image = Properties.Resources.button;
            ReturnToMenu.BackColor = Color.Transparent;
            ReturnToMenu.Font = new Font("Segoe UI", 9);
            ReturnToMenu.Location = new Point(20, Height - 75);
            ReturnToMenu.Text = "Cash Out";
            ReturnToMenu.Click += new System.EventHandler(Cashout_Click);
            Controls.Add(ReturnToMenu);

            PictureBox butt1 = new PictureBox();
            butt1.Image = Properties.Resources.button;
            butt1.Size = new Size(55, 22);
            butt1.SizeMode = PictureBoxSizeMode.StretchImage;
            butt1.Location = new Point(20, Height - 75);
            butt1.SendToBack();
            Controls.Add(butt1);

            Label BettingLabel = new Label();
            BettingLabel.Size = new Size(100, 50);
            BettingLabel.Location = new Point(Width / 2 - 50, Height / 2 - 85);
            BettingLabel.Font = new Font("Segoe UI", 30);
            BettingLabel.BackColor = Color.Transparent;
            BettingLabel.Text = " BET ";
            Controls.Add(BettingLabel);

            Label BalanceLabel = new Label();
            BalanceLabel.Size = new Size(150, 15);
            BalanceLabel.Location = new Point(Width / 2 - 55, Height / 2 - 30);
            BalanceLabel.Font = new Font("Segoe UI", 9);
            BalanceLabel.Text = "Game Balance: $" + ClientMain.MainPlayer.GameBalance;
            BalanceLabel.BackColor = Color.Transparent;
            Controls.Add(BalanceLabel);

            Label BetLabel = new Label();
            BetLabel.Size = new Size(20, 30);
            BetLabel.Location = new Point(Width / 2 - 52, Height / 2 - 10);
            BetLabel.Font = ClientGUI.FontSmaller;
            BetLabel.Text = "$";
            BetLabel.BackColor = Color.Transparent;
            Controls.Add(BetLabel);

            BetTextBox = new TextBox();
            BetTextBox.Size = new Size(70, 30);
            BetTextBox.Font = ClientGUI.FontSmaller;
            BetTextBox.Location = new Point(Width / 2 - 32, Height / 2 - 10);
            Controls.Add(BetTextBox);

            ErrorLabel = new Label();
            ErrorLabel.Size = new Size(200, 50);
            ErrorLabel.Location = new Point(Width / 2 - 37, Height / 2 + 75);
            ErrorLabel.Font = new Font("Segoe UI", 10);
            ErrorLabel.ForeColor = Color.Red;
            ErrorLabel.BackColor = Color.Transparent;
            Controls.Add(ErrorLabel);

            Button SubmitBet = new Button();
            SubmitBet.Size = new Size(70, 30);
            SubmitBet.BackColor = Color.White;
            SubmitBet.Location = new Point(Width / 2 - 32, Height / 2 + 25);
            SubmitBet.Font = new Font("Segoe UI", 10);
            SubmitBet.Text = "Ready";
            SubmitBet.Click += new System.EventHandler(SubmitBet_Click);
            Controls.Add(SubmitBet);

        }
    }
}

