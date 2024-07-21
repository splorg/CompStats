using System;
using System.Collections.Generic;
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
        public async Task<string> PostMatchStats(Match match)
        {
            string matchJson = JsonConvert.SerializeObject(match);
            StringContent content = new StringContent(matchJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(this.config.apiURL + "/stats", content);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseJson);
                return jsonResponse.id;
            }
            else
            {
                Console.WriteLine($"POST /stats: Request failed with status code: {response.StatusCode}");
                return null;
            }
        }

        public async void PostPlayers(List<Player> players, string matchId)
        {
            PostPlayerDTO dto = new PostPlayerDTO(players, matchId);
            string json = JsonConvert.SerializeObject(dto);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(this.config.apiURL + "/players", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"POST /players: Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
