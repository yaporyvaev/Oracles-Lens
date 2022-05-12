using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Telegram.BotCommands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class ChannelMessageHandler : IHostedService
    {
        private readonly TelegramBotClient _tgClient;
        private readonly CommandHandler _commandHandler;
        private readonly TelegramOptions _options;
        private readonly ILogger<ChannelMessageHandler> _logger;
        private CancellationTokenSource _cts;
        
        private string _botUserName;
        
        public ChannelMessageHandler(
            TelegramOptions options,
            ILogger<ChannelMessageHandler> logger,
            TelegramBotClient tgClient,
            CommandHandler commandHandler)
        {
            _options = options;
            _logger = logger;
            _tgClient = tgClient;
            _commandHandler = commandHandler;
        }

        #region IHostedService
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();
            _botUserName = _tgClient.GetMeAsync().GetAwaiter().GetResult().Username;

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

        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
        {
            if(string.IsNullOrEmpty(update.Message.Text)) return;

            if (update.Message.Text.StartsWith($"@{_botUserName}"))
            {
                await _commandHandler.Handle(update);
            }
        }
        
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
