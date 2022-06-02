using LeagueActivityBot.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Exceptions;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using Telegram.Bot;
using Telegram.Bot.Types;

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
                var respondMessage = await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken, disableNotification: true);
                if (respondMessage.MessageId == 0)
                {
                    throw new ClientException("ID NOL'");
                }
            }
        }
    }
}