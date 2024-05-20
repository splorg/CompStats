using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CompStats
{
    public class StatsService
    {
        private Config config;
        private HttpClient client;
        public StatsService(Config config)
        {
            this.config = config;
            this.client = new HttpClient();
        }
        public async Task<string> addMatch(string mapName, string team1, string team2, string gameType)
        {
            Match match = new Match(mapName, team1, team2, gameType);
            match.tournamentName = this.config.tournamentName;

            string matchJson = JsonConvert.SerializeObject(match);
            StringContent content = new StringContent(matchJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this.client.PostAsync(this.config.apiURL + "/match", content);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseJson);
                return jsonResponse.id;
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
