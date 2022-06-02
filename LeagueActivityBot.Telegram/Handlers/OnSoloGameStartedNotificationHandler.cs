using LeagueActivityBot.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Exceptions;
using LeagueActivityBot.Notifications.OnSoloGameStarted;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameStartedNotificationHandler : INotificationHandler<OnSoloGameStartedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClient _tgClient;

        public OnSoloGameStartedNotificationHandler(TelegramOptions options, TelegramBotClient tgClient)
        {
            _options = options;
            _tgClient = tgClient;
        }

        public async Task Handle(OnSoloGameStartedNotification notification, CancellationToken cancellationToken)
        {
            var messageBuilder = new OnSoloGameStartedMessageBuilder();
            var message = messageBuilder.Build(notification);

            var respondMessage = await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken, disableNotification: true);
            if (respondMessage.MessageId == 0)
            {
                throw new ClientException("ID NOL'");
            }
        }
    }
}