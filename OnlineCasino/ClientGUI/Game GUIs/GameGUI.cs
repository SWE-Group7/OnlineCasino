using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic.Players;

namespace ClientGUI.Game_GUIs
{
    public abstract class GameGUI
    {
        public enum OverallState
        {
            Waiting = 0,
            Playing,
            Distributing
        }
        public OverallState OS = OverallState.Waiting;

        public enum WaitingState
        {
            NoConnection = 0,
            TableFound,

        }
        public WaitingState WS = WaitingState.NoConnection;

        public enum GameState
        {
            Waiting = 0,
            Betting,
            Playing
        }
        public GameState GS = GameState.Waiting;

        public enum RoundEndState
        {
            Win = 0,
            Lose,
            Tie
        }
        public RoundEndState RES = RoundEndState.Tie;      

        protected System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

        protected int clientHeight;
        protected int clientWidth;

        public int clickX;
        public int clickY;
        public int hoverX;
        public int hoverY;

        public Player You;
        public List<Player> OtherPlayers;

        protected float sx = 0;
        protected bool sp = true;
        protected void JoiningTable_Draw(object sender, PaintEventArgs e)
        {
            Stopwatch.Start();
            e.Graphics.DrawRectangle(Pens.Black, clientWidth / 2 - 226, clientHeight / 2 - 126, 451, 251);
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));

            e.Graphics.DrawString("JOINING NEXT FREE TABLE", new Font("Segoe UI", 16), Brushes.Black, new Point(clientWidth / 2 - 140, clientHeight / 2 - 30));
            var t = e.Graphics.Transform;

            if (sp)
                sx += .01f;
            else
                sx -= .01f;
            if (sx < -.3) sp = true;
            if (sx > .3) sp = false;

            t.Shear(sx, 0);

            e.Graphics.Transform = t;
            e.Graphics.DrawString("......", new Font("Segoe UI", 12), Brushes.Black, new Point(clientWidth / 2, clientHeight / 2 + 10));

            CheckConnection();
        }
        protected void TableFound_Draw(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(clientWidth / 2 - 225, clientHeight / 2 - 125, 450, 250));
            e.Graphics.DrawString("Table found! Seating Players..", new Font("Segoe UI", 16), Brushes.Black, new Point(clientWidth / 2 - 150, clientHeight / 2 - 30));
        }

        protected void CheckConnection()
        {
            if (Stopwatch.ElapsedMilliseconds > 1000)
            {
                OS = OverallState.Playing;
                GS = GameState.Playing;

                Stopwatch.Stop();
                Stopwatch.Reset();
            }
        }
    }
}
