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
                        rollingInterval: RollingInterval.Day);

                // TODO: Вынуть получение настроек телеграма в общий экстеншн.
                // Или добавить клиент в контейнер.
                loggerConfiguration
                    .WriteTo.TeleSink(
                        logChatId: context.Configuration["App:Telegram:LogChatId"],
                        apiKey: context.Configuration["App:Telegram:ApiKey"]);
            });

            return hostBuilder;
        }
    }
}
