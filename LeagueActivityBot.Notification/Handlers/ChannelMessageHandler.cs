using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Notification.Handlers
{
    public class ChannelMessageHandler : IHostedService
    {
        private readonly TelegramBotClient _tgClient;
        private readonly NotificationOptions _options;
        private readonly ILogger<ChannelMessageHandler> _logger;
        private DateTime _lastStasMessage = DateTime.MinValue;
        private CancellationTokenSource _cts;

        public ChannelMessageHandler(
            NotificationOptions options,
            ILogger<ChannelMessageHandler> logger,
            TelegramBotClient tgClient)
        {
            _options = options;
            _logger = logger;
            _tgClient = tgClient;
        }

        #region IHostedService
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();

            _tgClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message }, Offset = -1 },
                _cts.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();

            return Task.CompletedTask;
        }
        #endregion

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update message, CancellationToken ct)
        {
            if (ShouldGreetStas(message))
            {
                _lastStasMessage = DateTime.UtcNow;
                await bot.SendTextMessageAsync(new ChatId(_options.TelegramChatId), "О, Стас пришёл (:", cancellationToken: ct);
            }
        }

        private bool ShouldGreetStas(Update message) => message.Message.From.Id == 501536687 && DateTime.UtcNow.AddHours(-3) > _lastStasMessage;

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(exception, errorMessage);

            return Task.CompletedTask;
        }
    }
}
