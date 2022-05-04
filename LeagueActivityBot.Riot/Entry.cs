using System;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Riot
{
    public static class Entry
    {
        [UsedImplicitly]
        public static IServiceCollection AddRiot<TOptions>([NotNull] this IServiceCollection serviceCollection, Action<TOptions> optionsAction)
            where TOptions : RiotClientOptions, new()
        {
            var options = new TOptions();
            optionsAction?.Invoke(options);

            var settings = new RiotClientOptions
            {
                ApiKey = options.ApiKey,
                BaseUrl = options.BaseUrl,
                SpectatorApiResource = options.SpectatorApiResource,
                SummonerApiResource = options.SummonerApiResource,
                LeagueApiResource = options.LeagueApiResource,
                MatchApiUrl = options.MatchApiUrl
            };
            serviceCollection.AddSingleton(settings);

            serviceCollection.AddHttpClient<IRiotClient, RiotHttpClient>(client =>
            {
                client.BaseAddress = new Uri(options.BaseUrl);
            });
            
            return serviceCollection;
        }
    }
}