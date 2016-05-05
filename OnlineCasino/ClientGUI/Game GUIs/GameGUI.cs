using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic.Players;
using ClientLogic.Games;
using ClientLogic;

namespace ClientGUI.Game_GUIs
{
    public abstract class GameGUI
    {
        protected System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

        protected int clientHeight;
        protected int clientWidth;

        public int clickX;
        public int clickY;
        public int hoverX;
        public int hoverY;

        protected Game CurrentGame
        {
            get
            {
                return ClientMain.MainGame;
            }
        }
        protected Player You
        {
            get
            {
                return ClientMain.MainPlayer;
            }
        }

        protected float sx = 0;
        protected bool sp = true;

        public GameGUI(int h, int w)
        {
            clientHeight = h;
            clientWidth = w;
        }


        public void JoiningTable_Draw(object sender, PaintEventArgs e)
        {


            e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));

            e.Graphics.DrawString("JOINING NEXT FREE TABLE", ClientGUI.FontSmall, Brushes.Black, new Point(clientWidth / 2 - 140, clientHeight / 2 - 30));
            var t = e.Graphics.Transform;

            if (sp)
                sx += .01f;
            else
                sx -= .01f;
            if (sx < -.3) sp = true;
            if (sx > .3) sp = false;

            t.Shear(sx, 0);

            e.Graphics.Transform = t;
            e.Graphics.DrawString("......", ClientGUI.FontSmaller, Brushes.Black, new Point(clientWidth / 2, clientHeight / 2 + 10));

            

            TableFound_Draw(sender, e);
        }
        public void TableFound_Draw(object sender, PaintEventArgs e)
        {

            e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
            e.Graphics.DrawString("Table found! Seating Players..", ClientGUI.FontSmall, Brushes.Black, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
                      
            CurrentGame.GS = SharedModels.Games.GameStates.Playing;
        }

        public bool CheckConnection()
        {
            if (Stopwatch.ElapsedMilliseconds > 2000)
            {
                                
                Stopwatch.Stop();
                Stopwatch.Reset();

                return true;
            }

            return false;
        }
    }
}
