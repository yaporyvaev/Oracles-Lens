using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Exceptions;
using LeagueActivityBot.Models;
using LeagueActivityBot.Riot.Models.Clash;
using LeagueActivityBot.Riot.Models.Config;
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
            
            throw new HttpClientException($"Получение результата на запрос информации об аккаунте. Код: {response.StatusCode}, сообщение: {responseContent}");
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

            throw new HttpClientException($"Получение результата на запрос информации о текущем матче. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        public async Task<MatchInfo> GetMatchInfo(long gameId)
        {
            var euwGameId = $"EUW1_{gameId}";
            return await GetMatchInfo(euwGameId);
        }

        public async Task<List<string>> GetMatchIds(string summonerPuuid, int skip, int take)
        {
            if (skip < 0) skip = 0;
            if (take > 100) take = 100;
            
            var response = await _httpClient.GetAsync($"{_setting.MatchApiUrl}/by-puuid/{summonerPuuid}/ids?start={skip}&count={take}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<string>>(responseContent);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<string>();
            }

            throw new HttpClientException($"Получение результата на запрос игр. Код: {response.StatusCode}, сообщение: {responseContent}");        
        }

        public async Task<MatchInfo> GetMatchInfo(string gameId)
        {
            var response = await _httpClient.GetAsync($"{_setting.MatchApiUrl}/{gameId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<MatchInfo>(responseContent);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw new HttpClientException($"Получение результата на запрос информации об игре. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        public async Task<LeagueInfo[]> GetLeagueInfo(string summonerId)
        {
            var response = await _httpClient.GetAsync($"{_setting.LeagueApiResource}/{summonerId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<LeagueInfo[]>(responseContent);
            }
            
            throw new HttpClientException($"Получение результата на запрос информации о лиге. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        public async Task<ClashInfo[]> GetClashSchedule()
        {
            var response = await _httpClient.GetAsync("lol/clash/v1/tournaments");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<ClashInfoRiotResponse[]>(responseContent);
                return _mapper.Map<ClashInfo[]>(responseModel);
            }
            
            throw new HttpClientException($"Получение результата на запрос информации о расписании турниров. Код: {response.StatusCode}, сообщение: {responseContent}");
        }

        private const string DefaultDataApiVersion = "13.12.1"; //Последняя версия на время разработки метода
        public async Task<string> GetLatestDataApiVersion()
        {
            var response = await _httpClient.GetAsync($"{_setting.DataDragonBaseUrl}/api/versions.json");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<string[]>(responseContent);
                return responseModel!.FirstOrDefault() ?? DefaultDataApiVersion;
            }
            
            throw new HttpClientException($"Получение результата на запрос информации о версиях API дата-сервиса. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
        
        public async Task<IEnumerable<ChampionInfo>> GetChampionsInfo()
        {
            var latestApiVersion = await GetLatestDataApiVersion();
            var response = await _httpClient.GetAsync($"{_setting.DataDragonBaseUrl}/cdn/{latestApiVersion}/data/en_US/champion.json");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseModel = JsonConvert.DeserializeObject<GetChampionsInfoRiotApiResponse>(responseContent);
                var properties = responseModel!.Data.Properties();

                var result = new List<ChampionInfo>();
                foreach (var prop in properties)
                {
                    var championInfo = prop.Value.ToObject(typeof(ChampionInfoRiotModel)) as ChampionInfoRiotModel;
                    result.Add(new ChampionInfo
                    {
                        Id = int.Parse(championInfo!.Key),
                        Name = championInfo.Name,
                        IconUrl = $"{_setting.DataDragonBaseUrl}/cdn/{latestApiVersion}/img/champion/{championInfo.Image.Full}"
                    });
                }

                return result;
            }
            
            throw new HttpClientException($"Получение результата на запрос информации о чемпионах. Код: {response.StatusCode}, сообщение: {responseContent}");
        }
    }
}