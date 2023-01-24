using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameStarted;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnGameStartedNotificationHandler : INotificationHandler<OnTeamGameStartedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;

        public OnGameStartedNotificationHandler(TelegramOptions options, TelegramBotClientWrapper telegramBotClientWrapper)
        {
            _options = options;
            _telegramBotClientWrapper = telegramBotClientWrapper;
        }

        public async Task Handle(OnTeamGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TimeSpan.FromHours(1), cancellationToken: cancellationToken);
        }
    }
}