using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.PeriodicBatching;
using Telegram.Bot;

namespace LeagueActivityBot.Host.Logging
{
    public static class TeleSinkExtensions
    {
        public static LoggerConfiguration TeleSink(
            this LoggerSinkConfiguration config,
            string apiKey,
            string logChatId,
            LogEventLevel minimumLevel = LogEventLevel.Error)
        {
            var tgClient = new TelegramBotClient(token: apiKey);
            var teleSink = new TeleSink(
                    tgClient: tgClient,
                    formatter: new JsonFormatter(),
                    chatId: logChatId,
                    minimumLevel: minimumLevel);

            return config.Sink(new PeriodicBatchingSink(teleSink, new PeriodicBatchingSinkOptions { }));
        }
    }
}
