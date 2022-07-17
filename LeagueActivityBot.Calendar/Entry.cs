using System;
using JetBrains.Annotations;
using LeagueActivityBot.Calendar.Integration;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Calendar
{
    public static class Entry
    {
        [UsedImplicitly]
        public static IServiceCollection AddCalendar<TOptions>([System.Diagnostics.CodeAnalysis.NotNull] this IServiceCollection serviceCollection, Action<TOptions> optionsAction)
            where TOptions : CalendarClientOptions, new()
        {
            var options = new TOptions();
            optionsAction?.Invoke(options);

            var settings = new CalendarClientOptions
            {
                ApiKey = options.ApiKey,
                BaseUrl = options.BaseUrl,
            };
            serviceCollection.AddSingleton(settings);

            serviceCollection.AddHttpClient<CalendarHttpClient>(client =>
            {
                client.BaseAddress = new Uri(options.BaseUrl);
            });
            
            serviceCollection.AddTransient<CalendarDataService>();

            return serviceCollection;
        }
    }
}