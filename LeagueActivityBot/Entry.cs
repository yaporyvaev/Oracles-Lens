using JetBrains.Annotations;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Notifications.OnTeamGameEnded;
using LeagueActivityBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot
{
    public static class Entry
    {
        [UsedImplicitly]
        public static IServiceCollection AddBot([NotNull] this IServiceCollection serviceCollection) 
        {
            serviceCollection.AddTransient<GameParticipantsHelper>();
            
            serviceCollection.AddTransient<OnSoloGameEndedTextMessageBuilder>();
            serviceCollection.AddTransient<OnTeamGameEndedTextMessageBuilder>();
            serviceCollection.AddTransient<OnSoloGameEndedWebAppMessageBuilder>();
            serviceCollection.AddTransient<OnTeamGameEndedWebAppMessageBuilder>();
            
            serviceCollection.AddTransient<StartGameChecker>();
            serviceCollection.AddTransient<EndGameChecker>();
            serviceCollection.AddTransient<GameService>();
            serviceCollection.AddTransient<StatisticService>();
            serviceCollection.AddTransient<LeagueService>();
            serviceCollection.AddTransient<SummonerService>();
            serviceCollection.AddTransient<ChampionInfoService>();
            serviceCollection.AddTransient<WeightsService>();

            return serviceCollection;
        }
    }
}