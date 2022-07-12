using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameEnded;
using LeagueActivityBot.Telegram.Extensions;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnGameEndedNotificationHandler : INotificationHandler<OnGameEndedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClient _tgClient;
        private readonly OnGameEndedMessageBuilder _messageBuilder;

        public OnGameEndedNotificationHandler(TelegramOptions options, TelegramBotClient tgClient, OnGameEndedMessageBuilder messageBuilder)
        {
            _options = options;
            _tgClient = tgClient;
            _messageBuilder = messageBuilder;
        }

        public async Task Handle(OnGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var message = await _messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _tgClient.SendTextMessage(_options.TelegramChatId, message);
            }
        }
    }
}