using System;
using System.Linq;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace LeagueActivityBot.Riot
{
    public class RiotTokenProvider
    {
        private const string RiotSettingsCacheKey = "RiotSettingsCacheKey";
        
        private readonly IRepository<RiotSettings> _riotSettingsRepository;
        private readonly IMemoryCache _cache;
        
        public RiotTokenProvider(IRepository<RiotSettings> riotSettingsRepository, IMemoryCache cache)
        {
            _riotSettingsRepository = riotSettingsRepository;
            _cache = cache;
        }

        public string GetKey()
        {
            if (!_cache.TryGetValue(RiotSettingsCacheKey, out RiotSettings cachedValue))
            {
                var settings = _riotSettingsRepository.GetAll()
                    .FirstOrDefault();

                if (settings != null)
                {
                    _cache.Set(RiotSettingsCacheKey, settings, TimeSpan.FromMinutes(5));
                    return settings.ApiKey;
                }

                throw new ArgumentException("Riot API key is not defined");
            }

            return cachedValue.ApiKey;
        }
    }
}