using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Entities.Enums;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;
using Microsoft.EntityFrameworkCore;
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
                .Include(s => s.LeagueInfos)
                .ToList();

            summoners = summoners
                .OrderByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.SoloDuo)?.Tier)
                .ThenBy(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.SoloDuo)?.Rank)
                .ThenByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.SoloDuo)?.LeaguePoints)
                .ThenByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.Flex)?.Tier)
                .ThenBy(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.Flex)?.Rank)
                .ThenByDescending(s => s.LeagueInfos.FirstOrDefault(l => l.LeagueType == LeagueType.Flex)?.LeaguePoints)
                .ThenBy(s => s.Name)
                .ToList();

            var sb = new StringBuilder();
            var counter = 1;
            foreach (var summoner in summoners)
            {
                sb.Append($"{counter}. <i><b>{summoner.Name}</b></i>");

                if (summoner.LeagueInfos != null && summoner.LeagueInfos.Any())
                {
                    foreach (var leagueInfo in summoner.LeagueInfos.OrderBy(l => l.LeagueType))
                    {
                        sb.Append(leagueInfo.LeagueType == LeagueType.SoloDuo ? " S\\D:" : " Flex:");
                        sb.Append($" {Models.LeagueInfo.GetTierStringRepresentation(leagueInfo.Tier)} {Models.LeagueInfo.GetRankStringRepresentation(leagueInfo.Rank)}, {leagueInfo.LeaguePoints} LP.");
                    }
                }
                
                sb.Append("\n");
                counter++;
            }
            
            return Task.FromResult(new CommandState(BotCommandsTypes.GetSummoners, commandOwnerId, new FinishCommandHandlingState(sb.ToString())));
        }
    }
}