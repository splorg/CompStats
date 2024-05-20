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
}
