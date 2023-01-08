using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnSoloGameStarted;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameStartedNotificationHandler : INotificationHandler<OnSoloGameStartedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;

        public OnSoloGameStartedNotificationHandler(TelegramOptions options, TelegramBotClientWrapper telegramBotClientWrapper)
        {
            _options = options;
            _telegramBotClientWrapper = telegramBotClientWrapper;
        }

        public async Task Handle(OnSoloGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnSoloGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TimeSpan.FromHours(1), cancellationToken: cancellationToken);
        }
    }
}