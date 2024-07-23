using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace CompStats
{
    public class StatsService
    {
        private readonly Config config;
        private readonly HttpClient client;
        public StatsService(Config config)
        {
            this.config = config;
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("X-API-KEY", config.apiKey);
        }
        public async Task<string> PostMatchStats(Match match)
        {
            string matchJson = JsonConvert.SerializeObject(match);
            StringContent content = new StringContent(matchJson, Encoding.UTF8, "application/json");

            Debug.Print("POSTING STATS TO " + this.config.apiURL + "/stats", 0, Debug.DebugColor.Green);

            HttpResponseMessage response = await this.client.PostAsync(this.config.apiURL + "/stats", content);

            if (!response.IsSuccessStatusCode)
            {
                Debug.Print($"POST /stats: Request failed with status code: {response.StatusCode}", 0, Debug.DebugColor.Red);
            }

            return null;
        }
    }
}
