using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
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
                .OrderBy(s => s.Name).ToList();

            var sb = new StringBuilder();
            var counter = 1;
            foreach (var summoner in summoners)
            {
                sb.Append($"{counter}. {summoner.Name}");
                if (!string.IsNullOrEmpty(summoner.RealName))
                {
                    sb.Append($" as {summoner.RealName}");
                }

                sb.Append("\n");
                counter++;
            }
            
            return Task.FromResult(new CommandState(BotCommandsTypes.GetSummoners, commandOwnerId, new FinishCommandHandlingState(sb.ToString())));
        }
    }
}