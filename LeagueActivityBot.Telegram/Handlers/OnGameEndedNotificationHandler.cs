using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameEnded;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnGameEndedNotificationHandler : INotificationHandler<OnTeamGameEndedNotification>
    {
        
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly OnTeamGameEndedMessageBuilder _messageBuilder;

        public OnGameEndedNotificationHandler(TelegramOptions options,  OnTeamGameEndedMessageBuilder messageBuilder, TelegramBotClientWrapper telegramBotClientWrapper)
        {
            _options = options;
            _messageBuilder = messageBuilder;
            _telegramBotClientWrapper = telegramBotClientWrapper;
        }

        public async Task Handle(OnTeamGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var message = _messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TimeSpan.FromHours(1), cancellationToken: cancellationToken);
            }
        }
    }
}