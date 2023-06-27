using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Notifications.OnTeamGameEnded;
using LeagueActivityBot.Telegram.Factories;
using LeagueActivityBot.Telegram.RecentMessages;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnTeamGameEndedNotificationHandler : INotificationHandler<OnTeamGameEndedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly TeamGameEndedNotificationBuilderFactory _notificationBuilderFactory;
        private readonly RecentGameNotificationMessageStore _recentGameNotificationMessageStore;

        public OnTeamGameEndedNotificationHandler(
            TelegramOptions options,  
            TelegramBotClientWrapper telegramBotClientWrapper, 
            RecentGameNotificationMessageStore recentGameNotificationMessageStore, 
            TeamGameEndedNotificationBuilderFactory notificationBuilderFactory)
        {
            _options = options;
            _telegramBotClientWrapper = telegramBotClientWrapper;
            _recentGameNotificationMessageStore = recentGameNotificationMessageStore;
            _notificationBuilderFactory = notificationBuilderFactory;
        }

        public async Task Handle(OnTeamGameEndedNotification notification, CancellationToken cancellationToken)
        {
            notification.WebAppUrl = _options.WebAppLink;
            
            var messageBuilder = _notificationBuilderFactory.GetBuilder();
            var message = messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TelegramMessageOptions.MessageTimeToLive, cancellationToken);
                
                var relatedMessage = _recentGameNotificationMessageStore.Get(notification.MatchInfo.Info.GameId);
                if (relatedMessage != null)
                    await _telegramBotClientWrapper.TgClient.DeleteMessageAsync(_options.TelegramChatId, relatedMessage.MessageId, cancellationToken);
            }
        }
    }
}