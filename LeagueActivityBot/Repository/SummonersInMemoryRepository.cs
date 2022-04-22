using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Repository
{
    public class SummonersInMemoryRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly BotOptions _botOptions;

        public IList<SummonerInfo> SummonersInfo { get; set; }
        
        public SummonersInMemoryRepository(IServiceScopeFactory serviceScopeFactory, BotOptions botOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _botOptions = botOptions;
            
            Initialize().GetAwaiter().GetResult();
        }

        public SummonerInfo FindByName(string summonerName)
        {
            return SummonersInfo.FirstOrDefault(s => s.Name == summonerName);
        }
        
        private async Task Initialize()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var riotClient = scope.ServiceProvider.GetService<IRiotClient>();

            SummonersInfo = new List<SummonerInfo>(_botOptions.SummonerNames.Length);
            foreach (var name in _botOptions.SummonerNames)
            {
                try
                {
                    var summoner = await riotClient.GetSummonerInfoByName(name);
                    SummonersInfo.Add(summoner);
                }
                catch
                {
                    //todo log
                }
            }
        }
    }
}