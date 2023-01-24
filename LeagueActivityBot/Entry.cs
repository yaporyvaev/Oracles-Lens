using System;
using JetBrains.Annotations;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Notifications.OnGameEnded;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Services;
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
            
            serviceCollection.AddTransient<GameParticipantsHelper>();
            
            serviceCollection.AddTransient<OnSoloGameEndedMessageBuilder>();
            serviceCollection.AddTransient<OnTeamGameEndedMessageBuilder>();
            
            serviceCollection.AddTransient<StartGameChecker>();
            serviceCollection.AddTransient<EndGameChecker>();
            serviceCollection.AddTransient<GameService>();
            serviceCollection.AddTransient<StatisticService>();
            serviceCollection.AddTransient<LeagueService>();

            return serviceCollection;
        }
    }
}