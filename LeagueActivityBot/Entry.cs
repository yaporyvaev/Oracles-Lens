using System;
using JetBrains.Annotations;
using LeagueActivityBot.Notifications.MessageBuilders;
using LeagueActivityBot.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot
{
    public static class Entry
    {
        [UsedImplicitly]
        public static IServiceCollection AddBot<TOptions>([NotNull] this IServiceCollection serviceCollection, Action<TOptions> optionsAction)
            where TOptions : BotOptions, new()
        {
            var options = new TOptions();
            optionsAction?.Invoke(options);

            var settings = new BotOptions
            {
                SummonerNames = options.SummonerNames
            };
            serviceCollection.AddSingleton(settings);
            
            serviceCollection.AddSingleton<GameInfoInMemoryRepository>();
            serviceCollection.AddTransient<GameParticipantsHelper>();
            serviceCollection.AddTransient<OnSoloGameEndedMessageBuilder>();

            return serviceCollection;
        }
    }
}