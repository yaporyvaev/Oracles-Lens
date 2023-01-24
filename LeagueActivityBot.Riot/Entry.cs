using System;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

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
                })
                .AddPolicyHandler(GetRetryPolicy());
            
            return serviceCollection;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}