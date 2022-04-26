using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications;
using LeagueActivityBot.Notifications.MessageBuilders;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Notification.Handlers
{
    public class OnSoloGameStartedNotificationHandler : INotificationHandler<OnSoloGameStartedNotification>
    {
        private readonly NotificationOptions _options;

        public OnSoloGameStartedNotificationHandler(NotificationOptions options)
        {
            _options = options;
        }
        
        public async Task Handle(OnSoloGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var tgClient = new TelegramBotClient(_options.TelegramBotApiKey);

            var messageBuilder = new OnSoloGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);
            
            await tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken, disableNotification: true);
        }
    }
}