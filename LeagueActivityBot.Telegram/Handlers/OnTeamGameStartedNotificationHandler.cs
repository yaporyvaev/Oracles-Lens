using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameStarted;
using LeagueActivityBot.Telegram.RecentMessages;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnTeamGameStartedNotificationHandler : INotificationHandler<OnTeamGameStartedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly RecentMessageStore _recentMessageStore;

        public OnTeamGameStartedNotificationHandler(TelegramOptions options, TelegramBotClientWrapper telegramBotClientWrapper, RecentMessageStore recentMessageStore)
        {
            _options = options;
            _telegramBotClientWrapper = telegramBotClientWrapper;
            _recentMessageStore = recentMessageStore;
        }

        public async Task Handle(OnTeamGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            var tgMessage = await _telegramBotClientWrapper.TgClient.SendTextMessageAsync(_options.TelegramChatId, message, cancellationToken: cancellationToken);
            _recentMessageStore.Save(new RecentGameNotificationMessage(tgMessage.MessageId, notification.GameId));
        }
    }
}