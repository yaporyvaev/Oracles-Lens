using System;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using Microsoft.Extensions.DependencyInjection;

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

                    entity.Puuid = summoner.Puuid;
                    await repository.Update(entity);
                }
                catch
                {
                    //todo log
                }
            }
        }
    }
}