using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameStarted;
using LeagueActivityBot.Telegram.Extensions;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnGameStartedNotificationHandler : INotificationHandler<OnGameStartedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClient _tgClient;

        public OnGameStartedNotificationHandler(TelegramOptions options, TelegramBotClient tgClient)
        {
            _options = options;
            _tgClient = tgClient;
        }

        public async Task Handle(OnGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            await _tgClient.SendTextMessage(_options.TelegramChatId, message);
        }
    }
}