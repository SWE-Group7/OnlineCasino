using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientLogic
{
    public partial class RouletteGUI : Form
    {
        public RouletteGUI()
        {
            InitialBet();
            //textBox2.Text;
            //InitializeComponent();
           // this.MouseClick += RouletteGUI_MouseClick;
        }

        private void RouletteGUI_MouseClick(object sender, MouseEventArgs e)
        {
            this.MouseClick += RouletteGUI_MouseClick;
            //throw new NotImplementedException();
            int X_value = e.X;
            int Y_value = e.Y;
            int case_X = 0;

            if (e.Clicks != 0)
            {
                //Application.Exit();
                Console.WriteLine(e.Location);

                //get case for x
                if (X_value >= 100 && X_value <= 150) { case_X = 1; }
                if (X_value > 150 && X_value <= 198) { case_X = 2; }
                if (X_value > 198 && X_value <= 248) { case_X = 3; }
                if (X_value > 248 && X_value <= 297) { case_X = 4; }
                if (X_value > 297 && X_value <= 347) { case_X = 5; }
                if (X_value > 347 && X_value <= 396) { case_X = 6; }
                if (X_value > 396 && X_value <= 446) { case_X = 7; }
                if (X_value > 446 && X_value <= 495) { case_X = 8; }
                if (X_value > 495 && X_value <= 546) { case_X = 9; }
                if (X_value > 546 && X_value <= 595) { case_X = 10; }
                if (X_value > 595 && X_value <= 643) { case_X = 11; }
                if (X_value > 643 && X_value <= 694) { case_X = 12; }
                if (X_value > 694 && X_value <= 742) { case_X = 13; }

                //sends the case for x and y position where it will be used in a switch statement
                user_Choice(case_X, Y_value);
            }

        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            RouletteGUI_MouseClick(sender, e);
        }

        private void user_Choice(int x, int y)
        {
            switch (x)
            {
                case 1:
                    if (y >= 202 && y <= 295) { label7.Text= "YOUR BET:\r\n\r\n#1\r\n"; }
                    if (y >= 107 && y < 202) { label7.Text = "YOUR BET:\r\n\r\n#2\r\n"; }
                    if (y >= 14 && y < 107) { label7.Text = "YOUR BET:\r\n\r\n#3\r\n"; }

                    break;

                case 2:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#4\r\n"; }
                    if (y >= 107 && y < 202) { label7.Text = "YOUR BET:\r\n\r\n#5\r\n"; }
                    if (y >= 14 && y < 107) { label7.Text = "YOUR BET:\r\n\r\n#6\r\n"; }
                    break;

                case 3:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#7\r\n"; }
                    if (y >= 107 && y < 202) { label7.Text = "YOUR BET:\r\n\r\n#8\r\n"; }
                    if (y >= 14 && y < 107) { label7.Text = "YOUR BET:\r\n\r\n#9\r\n"; }

                    break;

                case 4:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#10\r\n"; }
                    if (y >= 107 && y <= 202) { label7.Text = "YOUR BET:\r\n\r\n#11\r\n"; }
                    if (y >= 14 && y <= 107) { label7.Text = "YOUR BET:\r\n\r\n#12\r\n"; }
                    break;

                case 5:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#13\r\n"; }
                    if (y >= 107 && y <= 202) { label7.Text = "YOUR BET:\r\n\r\n#14\r\n"; }
                    if (y >= 14 && y <= 107) { label7.Text = "YOUR BET:\r\n\r\n#15\r\n"; }

                    break;

                case 6:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#16\r\n"; }
                    if (y >= 107 && y <= 202) { label7.Text = "YOUR BET:\r\n\r\n#17\r\n"; }
                    if (y >= 14 && y <= 107) { label7.Text = "YOUR BET:\r\n\r\n#18\r\n"; }
                    break;

                case 7:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#19\r\n"; }
                    if (y >= 107 && y <= 202) { label7.Text = "YOUR BET:\r\n\r\n#20\r\n"; }
                    if (y >= 14 && y <= 107) { label7.Text = "YOUR BET:\r\n\r\n#21\r\n"; }

                    break;

                case 8:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#22\r\n"; }
                    if (y >= 107 && y < 202) { label7.Text = "YOUR BET:\r\n\r\n#23\r\n"; }
                    if (y >= 14 && y < 107) { label7.Text = "YOUR BET:\r\n\r\n#24\r\n"; }

                    break;

                case 9:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#25\r\n"; }
                    if (y >= 107 && y < 202) { label7.Text = "YOUR BET:\r\n\r\n#26\r\n"; }
                    if (y >= 14 && y < 107) { label7.Text = "YOUR BET:\r\n\r\n#27\r\n"; }
                    break;

                case 10:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#28\r\n"; }
                    if (y >= 107 && y < 202) { label7.Text = "YOUR BET:\r\n\r\n#29\r\n"; }
                    if (y >= 14 && y < 107) { label7.Text = "YOUR BET:\r\n\r\n#30\r\n"; }

                    break;

                case 11:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#31\r\n"; }
                    if (y >= 107 && y <= 202) { label7.Text = "YOUR BET:\r\n\r\n#32\r\n"; }
                    if (y >= 14 && y <= 107) { label7.Text = "YOUR BET:\r\n\r\n#33\r\n"; }
                    break;

                case 12:
                    if (y >= 202 && y <= 295) { label7.Text = "YOUR BET:\r\n\r\n#34\r\n"; }
                    if (y >= 107 && y <= 202) { label7.Text = "YOUR BET:\r\n\r\n#35\r\n"; }
                    if (y >= 14 && y <= 107) { label7.Text = "YOUR BET:\r\n\r\n#36\r\n"; }

                    break;

                case 13:
                    label7.Text = "YOUR BET:\r\n\r\n2-to-1\r\n"; ;
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //required by designer class
        }

    //these are methods required or used for InitialBet() in Designer.cs
        private void pictureBox2_Click(object sender, MouseEventArgs e)
        {
           // Form1_MouseClick(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {  
            //resets background
            this.BackgroundImage = null;
            this.BackColor = System.Drawing.Color.ForestGreen;

            //start table
            InitializeComponent();
            Console.WriteLine(this.textBox1.Text); Console.WriteLine(this.textBox2.Text);

        }

        private void button2_Click(object sender, EventArgs e)//back button on start screen
        {
            //in the game this button should take the user back to the pick game page.
            Application.Exit(); Console.WriteLine("The back button was clicked"); 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)//allows user to change bet while in table screen
        {
            //create new form that will allow user to enter new bet
            ChangingBet();
        }

        private void Done_Click(object sender, EventArgs e)
        {
            //this.Close();
            textBox2.Text = input.Text;
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)//Exit Button on table screen
        {
            //Will exit application for now, but will work similar to back button which will take user back to main menu
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)//this is the spin button
        {
            Form video = new Form();
            //this.Hide();
            //AxShockwaveFlashObjects.AxShockwaveFlash Flash1;
            //Flash1 = new AxShockwaveFlashObjects.AxShockwaveFlash();
            //video.Size = new System.Drawing.Size(939, 540);
            //Flash1.Dock= DockStyle.Fill;
            //video.Controls.Add(Flash1);
            //video.Show();
            //try
            //{   
            //    Flash1.Movie = "https://www.youtube.com/v/dlrzUQRdDbs?autoplay=1";
            //    Flash1.Play();
            //}
            //catch (Exception exp) { Application.Exit(); Console.WriteLine("guess there was an exception" + exp.StackTrace); }

            //HOW TO MAKE VIDEO FORM DISAPPEAR AFTER VIDEO ENDS??
        }
    }
}

