using System;
using JetBrains.Annotations;
using LeagueActivityBot.Notification.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

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
            serviceCollection.AddHostedService<ChannelMessageHandler>();
            serviceCollection.AddTransient(_ => new TelegramBotClient(settings.TelegramBotApiKey));

            return serviceCollection;
        }
    }
}