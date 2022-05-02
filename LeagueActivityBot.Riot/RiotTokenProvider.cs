using System.Linq;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;

namespace LeagueActivityBot.Riot
{
    public class RiotTokenProvider
    {
        
        private readonly IRepository<RiotSettings> _riotSettingsRepository;
        
        public RiotTokenProvider(IRepository<RiotSettings> riotSettingsRepository)
        {
            _riotSettingsRepository = riotSettingsRepository;
        }

        public string GetKey()
        {
            var settings = _riotSettingsRepository.GetAll()
                .FirstOrDefault();

            return settings.ApiKey;
        }
    }
}