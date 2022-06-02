using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Exceptions;
using LeagueActivityBot.Notifications.OnGameStarted;
using Telegram.Bot;
using Telegram.Bot.Types;

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

            var respondMessage = await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken, disableNotification: true);
            if (respondMessage.MessageId == 0)
            {
                throw new ClientException("ID NOL'");
            }
        }
    }
}