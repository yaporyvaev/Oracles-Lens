using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameEnded;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
                await _tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, disableNotification: true, parseMode:ParseMode.Html);
            }
        }
    }
}