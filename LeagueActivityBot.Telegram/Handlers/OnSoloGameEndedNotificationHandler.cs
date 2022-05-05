using LeagueActivityBot.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly NotificationOptions _options;
        private readonly OnSoloGameEndedMessageBuilder _messageBuilder;
        private readonly TelegramBotClient _tgClient;

        public OnGameEndedNotificationHandler(
            NotificationOptions options,
            OnSoloGameEndedMessageBuilder onSoloGameEndedMessageBuilder,
            TelegramBotClient tgClient)
        {
            _options = options;
            _messageBuilder = onSoloGameEndedMessageBuilder;
            _tgClient = tgClient;
        }

        public async Task Handle(OnSoloGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var message = await _messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken, disableNotification: true);
            }
        }
    }
}