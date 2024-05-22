using System.Collections.Generic;

namespace CompStats
{
    public class Player
    {
        public string name { set; get; }
        public string virtual_id { set; get; }

        private Stats stats;

        public Player(string name, string virtual_id)
        {
            this.name = name;
            this.virtual_id = virtual_id;
        }

        public void SetStats(Stats stats)
        {
            this.stats = stats;
        }
    }

    public class PostPlayerDTO
    {
        public List<Player> players;
        public string matchId;

        public PostPlayerDTO(List<Player> players, string matchId)
        {
            this.players = players;
            this.matchId = matchId;
        }
    }
}
