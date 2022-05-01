using LeagueActivityBot.Notifications;
using LeagueActivityBot.Notifications.MessageBuilders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Notification.Handlers
{
    public class OnSoloGameStartedNotificationHandler : INotificationHandler<OnSoloGameStartedNotification>
    {
        private readonly NotificationOptions _options;
        private readonly TelegramBotClient _tgClient;

        public OnSoloGameStartedNotificationHandler(NotificationOptions options, TelegramBotClient tgClient)
        {
            _options = options;
            _tgClient = tgClient;
        }

        public async Task Handle(OnSoloGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnSoloGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken, disableNotification: true);
        }
    }
}