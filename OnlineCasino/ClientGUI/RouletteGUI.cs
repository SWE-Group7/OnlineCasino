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
                Application.Exit();
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
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #1"); }
                    if (y >= 107 && y < 202) { Console.WriteLine("you placed your bet on #2"); }
                    if (y >= 14 && y < 107) { Console.WriteLine("you placed your bet on #3"); }

                    break;

                case 2:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #4"); }
                    if (y >= 107 && y < 202) { Console.WriteLine("you placed your bet on #5"); }
                    if (y >= 14 && y < 107) { Console.WriteLine("you placed your bet on #6"); }
                    break;

                case 3:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #7"); }
                    if (y >= 107 && y < 202) { Console.WriteLine("you placed your bet on #8"); }
                    if (y >= 14 && y < 107) { Console.WriteLine("you placed your bet on #9"); }

                    break;

                case 4:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #10"); }
                    if (y >= 107 && y <= 202) { Console.WriteLine("you placed your bet on #11"); }
                    if (y >= 14 && y <= 107) { Console.WriteLine("you placed your bet on #12"); }
                    break;

                case 5:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #13"); }
                    if (y >= 107 && y <= 202) { Console.WriteLine("you placed your bet on #14"); }
                    if (y >= 14 && y <= 107) { Console.WriteLine("you placed your bet on #15"); }

                    break;

                case 6:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #16"); }
                    if (y >= 107 && y <= 202) { Console.WriteLine("you placed your bet on #17"); }
                    if (y >= 14 && y <= 107) { Console.WriteLine("you placed your bet on #18"); }
                    break;

                case 7:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #19"); }
                    if (y >= 107 && y <= 202) { Console.WriteLine("you placed your bet on #20"); }
                    if (y >= 14 && y <= 107) { Console.WriteLine("you placed your bet on #21"); }

                    break;

                case 8:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #22"); }
                    if (y >= 107 && y < 202) { Console.WriteLine("you placed your bet on #23"); }
                    if (y >= 14 && y < 107) { Console.WriteLine("you placed your bet on #24"); }

                    break;

                case 9:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #25"); }
                    if (y >= 107 && y < 202) { Console.WriteLine("you placed your bet on #26"); }
                    if (y >= 14 && y < 107) { Console.WriteLine("you placed your bet on #27"); }
                    break;

                case 10:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #28"); }
                    if (y >= 107 && y < 202) { Console.WriteLine("you placed your bet on #29"); }
                    if (y >= 14 && y < 107) { Console.WriteLine("you placed your bet on #30"); }

                    break;

                case 11:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #31"); }
                    if (y >= 107 && y <= 202) { Console.WriteLine("you placed your bet on #32"); }
                    if (y >= 14 && y <= 107) { Console.WriteLine("you placed your bet on #33"); }
                    break;

                case 12:
                    if (y >= 202 && y <= 295) { Console.WriteLine("you placed your bet on #34"); }
                    if (y >= 107 && y <= 202) { Console.WriteLine("you placed your bet on #35"); }
                    if (y >= 14 && y <= 107) { Console.WriteLine("you placed your bet on #36"); }

                    break;

                case 13:
                    Console.WriteLine("you picked one to of the 2-to-1 choices");
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
            //hides labels from start screen
            this.label2.Hide();
            this.label3.Hide();
            this.button1.Hide();
            this.button2.Hide();
            this.textBox1.Hide();
            this.textBox2.Hide();
            //start table form
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
            //textBox2.Text= "400";
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
            AxShockwaveFlashObjects.AxShockwaveFlash Flash1;
            Flash1 = new AxShockwaveFlashObjects.AxShockwaveFlash();
            video.Size = new System.Drawing.Size(939, 540);
            Flash1.Dock= DockStyle.Fill;
            video.Controls.Add(Flash1);
            video.Show();
            try
            {   
                Flash1.Movie = "https://www.youtube.com/v/dlrzUQRdDbs?autoplay=1";
                Flash1.Play();
            }
            catch (Exception exp) { Application.Exit(); Console.WriteLine("guess there was an exception" + exp.StackTrace); }

            //HOW TO MAKE VIDEO FORM DISAPPEAR AFTER VIDEO ENDS??
        }
    }
}

