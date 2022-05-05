using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LeagueActivityBot
{
    public class SummonersInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var riotClient = scope.ServiceProvider.GetService<IRiotClient>();
            var repository = scope.ServiceProvider.GetService<IRepository<Summoner>>();
            var logger = scope.ServiceProvider.GetService<ILogger<SummonersInitializer>>();

            var summoners = repository.GetAll().ToList();
            
            foreach (var summoner in summoners)
            {
                try
                {
                    var leagueInfo = (await riotClient.GetLeagueInfo(summoner.SummonerId))
                        .FirstOrDefault(l => l.QueueType == QueueType.RankedSolo);

                    if (leagueInfo != null)
                    {
                        summoner.LeaguePoints = leagueInfo.LeaguePoints;
                        summoner.Tier = leagueInfo.GetTierIntegerRepresentation();
                        summoner.Rank = leagueInfo.GetRankIntegerRepresentation();
                        await repository.Update(summoner);
                    }
                }
                catch(Exception e)
                {
                    logger.LogError($"Summoner {summoner.Name} initialization failed", e);
                }
            }
        }
    }
}