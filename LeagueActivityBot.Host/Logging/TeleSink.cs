using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Host.Logging
{
    public class TeleSink : IBatchedLogEventSink
    {
        private const int DefaultWriteBufferCapacity = 256;

        private readonly LogEventLevel _minimumLevel;
        private readonly TelegramBotClient _tgClient;
        private readonly ITextFormatter _formatter;
        private readonly long _chatId;

        public TeleSink(TelegramBotClient tgClient, ITextFormatter formatter, long chatId, LogEventLevel minimumLevel)
        {
            _tgClient = tgClient;
            _formatter = formatter;
            _chatId = chatId;
            _minimumLevel = minimumLevel;
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            foreach (var logEvent in batch)
            {
                await Emit(logEvent);
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }

        private async Task Emit(LogEvent logEvent)
        {
            if (logEvent.Level < _minimumLevel) return;
            if (_chatId == default) return;

            var buffer = new StringWriter(new StringBuilder(DefaultWriteBufferCapacity));
            _formatter.Format(logEvent, buffer);
            var formattedLogEventText = buffer.ToString();

            await _tgClient.SendTextMessageAsync(new ChatId(_chatId), formattedLogEventText, disableNotification: true);
        }
    }
}
