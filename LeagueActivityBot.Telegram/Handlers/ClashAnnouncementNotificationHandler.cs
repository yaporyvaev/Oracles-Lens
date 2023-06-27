using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnClashScheduleReceived;
using LeagueActivityBot.Telegram.Extensions;
using Telegram.Bot;
namespace LeagueActivityBot.Telegram.Handlers
{
    public class ClashAnnouncementNotificationHandler : INotificationHandler<ClashAnnouncementNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClient _tgClient;
        
        public ClashAnnouncementNotificationHandler(TelegramOptions options, TelegramBotClient tgClient)
        {
            _options = options;
            _tgClient = tgClient;
        }

        public async Task Handle(ClashAnnouncementNotification notification, CancellationToken cancellationToken)
        {
            var message = ClashAnnouncementMessageBuilder.Build(notification);
            
            await _tgClient.SendTextMessage(_options.TelegramChatId, message);
        }
    }
}