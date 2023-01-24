using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LeagueActivityBot
{
    public class SummonersInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetService<IRepository<Summoner>>();
            var leagueService = scope.ServiceProvider.GetService<LeagueService>();
            var logger = scope.ServiceProvider.GetService<ILogger<SummonersInitializer>>();

            var summoners = repository.GetAll()
                .Include(s => s.LeagueInfos)
                .ToList();
            
            foreach (var summoner in summoners)
            {
                try
                {
                    await leagueService.UpdateLeague(summoner);
                }
                catch(Exception e)
                {
                    logger.LogError($"Summoner {summoner.Name} initialization failed", e);
                }
            }
        }
    }
}