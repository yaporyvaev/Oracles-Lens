using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Exceptions;
using LeagueActivityBot.Models;
using Newtonsoft.Json;

namespace LeagueActivityBot.Riot
{
    public class RiotHttpClient : IRiotClient
    {
        private readonly HttpClient _httpClient;
        private readonly RiotClientOptions _setting;

        public RiotHttpClient(HttpClient httpClient, RiotClientOptions setting)
        {
            _httpClient = httpClient;
            _setting = setting;

            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", setting.ApiKey);
        }

        public async Task<SummonerInfo> GetSummonerInfoByName(string summonerName)
        {
            var response = await _httpClient.GetAsync($"{_setting.SummonerApiResource}/{summonerName}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<SummonerInfo>(responseContent);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            
            throw new ClientException(
                $"Получение результата на запрос информации об аккаунте. Код: {response.StatusCode}, сообщение: {responseContent}");
        }

        public async Task<CurrentGameInfo> GetCurrentGameInfo(string summonerId)
        {
            var response = await _httpClient.GetAsync($"{_setting.SpectatorApiResource}/{summonerId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<CurrentGameInfo>(responseContent);
                if (result != null) result.IsInGameNow = true;

                return result;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new CurrentGameInfo
                {
                    IsInGameNow = false
                };
            }

            throw new ClientException(
                $"Получение результата на запрос информации о текущем матче. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        public async Task<MatchInfo> GetMatchInfo(long gameId)
        {
            var response = await _httpClient.GetAsync($"{_setting.MatchApiUrl}/EUW1_{gameId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<MatchInfo>(responseContent);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw new ClientException(
                $"Получение результата на запрос информации об игре. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        public async Task<LeagueInfo[]> GetLeagueInfo(string summonerId)
        {
            var response = await _httpClient.GetAsync($"{_setting.LeagueApiResource}/{summonerId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<LeagueInfo[]>(responseContent);
            }
            
            throw new ClientException(
                $"Получение результата на запрос информации о лиге. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
    }
}