using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ClientLogic;
using ClientLogic.Players;

namespace ClientLogic.Game_GUIs
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
            Betting
        }
        public WaitingState WS = WaitingState.NoConnection;

        public enum GameState
        {
            Waiting = 0,
            Playing
        }
        public GameState PS = GameState.Waiting;

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

        protected void CheckConnection()
        {
            if (Stopwatch.ElapsedMilliseconds > 1000)
            {
                OS = OverallState.Playing;
                PS = GameState.Playing;

                Stopwatch.Stop();
                Stopwatch.Reset();
            }
        }
    }
}
