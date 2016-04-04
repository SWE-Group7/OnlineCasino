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
                    {
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 226, Height / 2 - 176, 451, 251);
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 225, Height / 2 - 175, 450, 250));
                    }
                    break;
                case State.Register:
                    {
                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 225, Height / 2 - 175, 450, 250));
                    }
                    break;
                case State.Menu:
                    {
                        e.Graphics.Clear(Color.White);
                        e.Graphics.DrawRectangle(Pens.Black, Width / 2 - 226, Height / 2 - 176, 451, 268);

                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(Width / 2 - 225, Height / 2 - 175, 600, 300));
                    }
                    break;
                case State.Game:
                    { }
                    break;
            }
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
