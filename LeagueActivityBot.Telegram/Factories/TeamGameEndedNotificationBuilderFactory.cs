using System;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Notifications.OnTeamGameEnded;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.Factories
{
    [UsedImplicitly]
    public class TeamGameEndedNotificationBuilderFactory
    {
        private readonly TelegramOptions _options;
        private readonly IServiceProvider _serviceProvider;
        
        public TeamGameEndedNotificationBuilderFactory(TelegramOptions options, IServiceProvider serviceProvider)
        {
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public INotificationBuilder GetBuilder()
        {
            if (_options.UseWebAppMatchResults)
            {
                return _serviceProvider.GetService<OnTeamGameEndedWebAppMessageBuilder>();
            }
            return _serviceProvider.GetService<OnTeamGameEndedTextMessageBuilder>();

        }
    }
}