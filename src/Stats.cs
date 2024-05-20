using System;
using System.Collections.Generic;
using System.Text;

namespace CompStats
{
    public class Stats
    {
        public int kills { set; get; }
        public int deaths { set; get; }
        public int assists { set; get; }
        public int score { set; get; }

        private bool win = false;

        public Stats(int kills, int deaths, int assists, int score)
        {
            this.kills = kills;
            this.deaths = deaths;
            this.assists = assists;
            this.score = score;
        }

        public void SetWinner() { this.win = true; }
    }
}
