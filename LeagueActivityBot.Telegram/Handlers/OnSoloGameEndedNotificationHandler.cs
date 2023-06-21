using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Telegram.RecentMessages;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly OnSoloGameEndedMessageBuilder _messageBuilder;
        private readonly RecentGameNotificationMessageStore _recentGameNotificationMessageStore;

        public OnSoloGameEndedNotificationHandler(
            TelegramOptions options,
            OnSoloGameEndedMessageBuilder onSoloGameEndedMessageBuilder,
            TelegramBotClientWrapper telegramBotClientWrapper, RecentGameNotificationMessageStore recentGameNotificationMessageStore)
        {
            _options = options;
            _messageBuilder = onSoloGameEndedMessageBuilder;
            _telegramBotClientWrapper = telegramBotClientWrapper;
            _recentGameNotificationMessageStore = recentGameNotificationMessageStore;
        }

        public async Task Handle(OnSoloGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var message = _messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TelegramMessageOptions.MessageTimeToLive, cancellationToken);
                
                var relatedMessage = _recentGameNotificationMessageStore.Get(notification.MatchInfo.Info.GameId);
                if (relatedMessage != null)
                    await _telegramBotClientWrapper.TgClient.DeleteMessageAsync(_options.TelegramChatId, relatedMessage.MessageId, cancellationToken);
            }
        }
    }
}