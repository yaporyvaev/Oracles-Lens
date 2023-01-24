using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnSoloGameEnded;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly OnSoloGameEndedMessageBuilder _messageBuilder;

        public OnSoloGameEndedNotificationHandler(
            TelegramOptions options,
            OnSoloGameEndedMessageBuilder onSoloGameEndedMessageBuilder,
            TelegramBotClientWrapper telegramBotClientWrapper)
        {
            _options = options;
            _messageBuilder = onSoloGameEndedMessageBuilder;
            _telegramBotClientWrapper = telegramBotClientWrapper;
        }

        public async Task Handle(OnSoloGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var message = _messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TimeSpan.FromHours(1), cancellationToken: cancellationToken);
            }
        }
    }
}