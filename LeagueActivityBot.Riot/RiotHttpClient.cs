using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Exceptions;
using LeagueActivityBot.Models;
using LeagueActivityBot.Riot.Models.Clash;
using Newtonsoft.Json;

namespace LeagueActivityBot.Riot
{
    public class RiotHttpClient : IRiotClient
    {
        private readonly HttpClient _httpClient;
        private readonly RiotClientOptions _setting;
        private readonly IMapper _mapper;

        public RiotHttpClient(HttpClient httpClient, RiotClientOptions setting, IMapper mapper)
        {
            _httpClient = httpClient;
            _setting = setting;
            _mapper = mapper;

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

        public async Task<SpectatorGameInfo> GetCurrentGameInfo(string summonerId)
        {
            var response = await _httpClient.GetAsync($"{_setting.SpectatorApiResource}/{summonerId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<SpectatorGameInfo>(responseContent);
                if (result != null) result.IsInGameNow = true;

                return result;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new SpectatorGameInfo
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
        
        public async Task<ClashInfo[]> GetClashSchedule()
        {
            var response = await _httpClient.GetAsync("lol/clash/v1/tournaments");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<ClashInfoResponseModel[]>(responseContent);
                return _mapper.Map<ClashInfo[]>(responseModel);
            }
            
            throw new ClientException(
                $"Получение результата на запрос информации о расписаеии турниров. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
    }
}