using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Telegram.Extensions;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly OnSoloGameEndedMessageBuilder _messageBuilder;
        private readonly TelegramBotClient _tgClient;

        public OnSoloGameEndedNotificationHandler(
            TelegramOptions options,
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
                await _tgClient.SendTextMessage(_options.TelegramChatId, message);
            }
        }
    }
}