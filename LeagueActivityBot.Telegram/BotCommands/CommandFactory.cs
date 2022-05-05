using System;
using LeagueActivityBot.Telegram.BotCommands.Abstractions;
using LeagueActivityBot.Telegram.BotCommands.AddSummoner;
using LeagueActivityBot.Telegram.BotCommands.Cancel;
using LeagueActivityBot.Telegram.BotCommands.RemoveSummoner;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.BotCommands
{
    public class CommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseCommand Create(string commandType)
        {
            return commandType switch
            {
                BotCommandsTypes.AddSummoner => _serviceProvider.GetService<AddSummonerCommand>(),
                BotCommandsTypes.RemoveSummoner => _serviceProvider.GetService<RemoveSummonerCommand>(),
                BotCommandsTypes.Cancel => _serviceProvider.GetService<CancelCommand>(),
                _ => null
            };
        }
    }
}