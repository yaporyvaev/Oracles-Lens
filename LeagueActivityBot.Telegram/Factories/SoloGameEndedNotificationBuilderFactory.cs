using System;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Telegram.Factories
{
    [UsedImplicitly]
    public class SoloGameEndedNotificationBuilderFactory
    {
        private readonly TelegramOptions _options;
        private readonly IServiceProvider _serviceProvider;
        
        public SoloGameEndedNotificationBuilderFactory(TelegramOptions options, IServiceProvider serviceProvider)
        {
            _options = options;
            _serviceProvider = serviceProvider;
        }

        public INotificationBuilder GetBuilder()
        {
            if (_options.UseWebAppMatchResults)
            {
                return _serviceProvider.GetService<OnSoloGameEndedWebAppMessageBuilder>();
            }

            return _serviceProvider.GetService<OnSoloGameEndedTextMessageBuilder>();
        }
    }
}