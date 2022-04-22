using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications;
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
            
            var message = $"{notification.SummonerName} крыса, играет в соло!";
            await tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken);
        }
    }
}