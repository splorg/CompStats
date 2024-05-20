namespace CompStats
{
    public class Match
    {
        private string mapName;
        private string team1;
        private string team2;
        private string gameType;
        
        public string tournamentName { get; set; }
        public Match(string mapName, string team1, string team2, string gameType)
        {
            this.mapName = mapName;
            this.team1 = team1;
            this.team2 = team2;
            this.gameType = gameType;
        }
    }
}
