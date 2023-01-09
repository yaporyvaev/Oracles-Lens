using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Models;
using LeagueActivityBot.Services;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.GeneralStates;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.BotCommands.GetStatistic
{
    [UsedImplicitly]
    public class GetStatisticCommand : BaseCommand
    {
        private readonly IServiceProvider _serviceProvider;
        
        public GetStatisticCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public override async Task<CommandState> Handle(long commandOwnerId, string payload)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var statisticService = serviceScope.ServiceProvider.GetService<StatisticService>();

            int.TryParse(payload, out var days);
            if (days < 0) days = 0;
            else if (days > 10000) days = 10000;

            IEnumerable<SummonerStatistic> statistics;
            if (days == 0)
            {
                statistics = await statisticService.GetStatistic();
            }
            else
            {
                statistics = await statisticService.GetStatistic(days);
            }
            
            var sb = new StringBuilder();
            var summonerStatistics = statistics as SummonerStatistic[] ?? statistics.ToArray();
            if (summonerStatistics.Any())
            {
                var counter = 1;
                foreach (var statistic in summonerStatistics.OrderByDescending(s => s.WinRate))
                {
                    sb.Append($"{counter}. <i><b>{statistic.SummonerName}</b></i> wins: {statistic.Wins}, loses: {statistic.Loses}, WR: {Math.Round(statistic.WinRate)}%\n");
                    counter++;
                }
            }
            else
            {
                sb.Append("New day, new wins!");
            }

            return new CommandState(BotCommandsTypes.GetStatistic, commandOwnerId, new FinishCommandHandlingState(sb.ToString()));
        }
    }
}