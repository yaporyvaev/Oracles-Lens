using System;
using JetBrains.Annotations;
using LeagueActivityBot.Notification.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Notification
{
    public static class Entry
    {
        [UsedImplicitly]
        public static IServiceCollection AddNotifications<TOptions>([NotNull] this IServiceCollection serviceCollection, Action<TOptions> optionsAction)
            where TOptions : NotificationOptions, new()
        {
            var options = new TOptions();
            optionsAction?.Invoke(options);

            var settings = new NotificationOptions
            {
                TelegramBotApiKey = options.TelegramBotApiKey,
                TelegramChatId = options.TelegramChatId
            };
            serviceCollection.AddSingleton(settings);

            var channelMessageHandler = new ChannelMessageHandler(settings);
            channelMessageHandler.StartHandling();
            serviceCollection.AddSingleton(channelMessageHandler);
            
            return serviceCollection;
        }
    }
}