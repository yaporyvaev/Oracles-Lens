using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.BotCommands.GetSummoners
{
    [UsedImplicitly]
    public class GetSummonersCommand : BaseCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public GetSummonersCommand(CommandStateStore stateStore, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task<CommandState> Handle(long commandOwnerId, string payload)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var repository = serviceScope.ServiceProvider.GetService<IRepository<Summoner>>();

            var summoners = repository.GetAll()
                .OrderByDescending(s => s.Tier).ThenBy(s => s.Rank).ThenByDescending(s => s.LeaguePoints).ToList();

            var sb = new StringBuilder();
            var counter = 1;
            foreach (var summoner in summoners)
            {
                sb.Append($"{counter}. {summoner.Name}");

                if (summoner.Rank != 0 && summoner.Tier != 0)
                {
                    sb.Append($". {LeagueInfo.GetTierStringRepresentation(summoner.Tier)} {LeagueInfo.GetRankStringRepresentation(summoner.Rank)}, {summoner.LeaguePoints} LP.");
                }
                
                sb.Append("\n");
                counter++;
            }
            
            return Task.FromResult(new CommandState(BotCommandsTypes.GetSummoners, commandOwnerId, new FinishCommandHandlingState(sb.ToString())));
        }
    }
}