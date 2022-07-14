using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
            
            var statistics = (await statisticService.GetDailyStatistic()).ToArray();
            var sb = new StringBuilder();

            if (statistics.Any())
            {
                var counter = 1;
                foreach (var statistic in statistics.OrderByDescending(s => s.WinRate))
                {
                    sb.Append($"{counter}. <i><b>{statistic.SummonerName}</b></i> wins: {statistic.Wins}, loses: {statistic.Loses}, WR: {Math.Round(statistic.WinRate)}%\n");
                    counter++;
                }
            }
            else
            {
                sb.Append("There is no statistic for current day.");
            }

            return new CommandState(BotCommandsTypes.GetStatistic, commandOwnerId, new FinishCommandHandlingState(sb.ToString()));
        }
    }
}