using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnSoloGameStarted;
using LeagueActivityBot.Telegram.RecentMessages;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameStartedNotificationHandler : INotificationHandler<OnSoloGameStartedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly RecentGameNotificationMessageStore _recentGameNotificationMessageStore;

        public OnSoloGameStartedNotificationHandler(TelegramOptions options, TelegramBotClientWrapper telegramBotClientWrapper, RecentGameNotificationMessageStore recentGameNotificationMessageStore)
        {
            _options = options;
            _telegramBotClientWrapper = telegramBotClientWrapper;
            _recentGameNotificationMessageStore = recentGameNotificationMessageStore;
        }

        public async Task Handle(OnSoloGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnSoloGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            var tgMessage = await _telegramBotClientWrapper.TgClient.SendTextMessageAsync(_options.TelegramChatId, message, ParseMode.Html, cancellationToken: cancellationToken);
            _recentGameNotificationMessageStore.Save(new RecentGameNotificationMessage(tgMessage.MessageId, notification.GameId));        
        }
    }
}