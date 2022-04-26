using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Notifications;
using LeagueActivityBot.Notifications.MessageBuilders;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Notification.Handlers
{
    public class OnGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly NotificationOptions _options;
        private readonly OnSoloGameEndedMessageBuilder _messageBuilder;

        public OnGameEndedNotificationHandler(NotificationOptions options, OnSoloGameEndedMessageBuilder onSoloGameEndedMessageBuilder)
        {
            _options = options;
            _messageBuilder = onSoloGameEndedMessageBuilder;
        }

        public async Task Handle(OnSoloGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var tgClient = new TelegramBotClient(_options.TelegramBotApiKey);
            
            var message = await _messageBuilder.Build(notification);
            
            if (!string.IsNullOrEmpty(message))
            {
                await tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken,disableNotification: true);
            }
        }
    }
}