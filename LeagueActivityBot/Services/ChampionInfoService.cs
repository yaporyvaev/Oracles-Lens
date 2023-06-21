using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LeagueActivityBot.Services
{
    public class ChampionInfoService
    {
        private readonly IRiotClient _riotClient;
        private readonly IMemoryCache _memoryCache;

        public ChampionInfoService(IRiotClient riotClient, IMemoryCache memoryCache)
        {
            _riotClient = riotClient;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetChampionIconUrl(int championId)
        {
            var keyValueChampionsInfo = _memoryCache.Get<Dictionary<int, ChampionInfo>>(GetCacheKey(championId));

            if (keyValueChampionsInfo == null)
            {
                var championsInfo = await _riotClient.GetChampionsInfo();
                keyValueChampionsInfo = championsInfo.ToDictionary(k => k.Id, v => v);
                
                _memoryCache.Set(GetCacheKey(championId), keyValueChampionsInfo, TimeSpan.FromHours(2));
            }

            return keyValueChampionsInfo[championId].IconUrl;
        }
        
        private static string GetCacheKey(int championKey)
        {
            return $"champ_info_{championKey}";
        }
    }
}