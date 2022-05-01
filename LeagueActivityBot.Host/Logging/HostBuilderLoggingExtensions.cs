using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace LeagueActivityBot.Host.Logging
{
    public static class HostBuilderLoggingExtensions
    {
        public static IHostBuilder UseDefaultSerilog(
            this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                   .ReadFrom.Configuration(context.Configuration)
                   .ReadFrom.Services(services)
                   .Enrich.FromLogContext()
                   .WriteTo.Console()
                   .WriteTo.File(
                        new RenderedCompactJsonFormatter(),
                        context.Configuration["Serilog:File:Path"],
                        rollingInterval: RollingInterval.Day)
                   .WriteTo.TeleSink(services);
            });

            return hostBuilder;
        }
    }
}
