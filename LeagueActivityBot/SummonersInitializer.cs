using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
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
            var botOptions = scope.ServiceProvider.GetService<BotOptions>();
            var logger = scope.ServiceProvider.GetService<ILogger<SummonersInitializer>>();

            foreach (var name in botOptions.SummonerNames)
            {
                try
                {
                    var summoner = await riotClient.GetSummonerInfoByName(name);
                    var entity = repository.GetAll().FirstOrDefault(s => s.Name == name);

                    if (entity == null)
                    {
                        await repository.Add(new Summoner
                        {
                            Name = name,
                            AccountId = summoner.AccountId,
                            SummonerId = summoner.Id,
                            Puuid = summoner.Puuid,
                        });
                        
                        continue;
                    }

                    entity.AccountId = summoner.AccountId;
                    entity.SummonerId = summoner.Id;
                    entity.Puuid = summoner.Puuid;
                    await repository.Update(entity);
                }
                catch(Exception e)
                {
                    logger.LogError($"Summoner {name} initialization failed", e);
                }
            }
        }
    }
}