using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Telegram.Factories;
using LeagueActivityBot.Telegram.RecentMessages;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnSoloGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly RecentGameNotificationMessageStore _recentGameNotificationMessageStore;
        private readonly SoloGameEndedNotificationBuilderFactory _notificationBuilderFactory;

        public OnSoloGameEndedNotificationHandler(
            TelegramOptions options,
            TelegramBotClientWrapper telegramBotClientWrapper, 
            RecentGameNotificationMessageStore recentGameNotificationMessageStore, SoloGameEndedNotificationBuilderFactory notificationBuilderFactory)
        {
            _options = options;
            _telegramBotClientWrapper = telegramBotClientWrapper;
            _recentGameNotificationMessageStore = recentGameNotificationMessageStore;
            _notificationBuilderFactory = notificationBuilderFactory;
        }

        public async Task Handle(OnSoloGameEndedNotification notification, CancellationToken cancellationToken)
        {
            notification.WebAppUrl = _options.WebAppLink;
            
            var messageBuilder = _notificationBuilderFactory.GetBuilder();
            var message = messageBuilder.Build(notification);
            
            if (!string.IsNullOrEmpty(message))
            {
                // _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TelegramMessageOptions.MessageTimeToLive, cancellationToken);
                await _telegramBotClientWrapper.TgClient.SendTextMessageAsync(_options.TelegramChatId, message, ParseMode.Html, disableWebPagePreview:true, disableNotification:true, cancellationToken:cancellationToken);
                
                var relatedMessage = _recentGameNotificationMessageStore.Get(notification.MatchInfo.Info.GameId);
                if (relatedMessage != null)
                    await _telegramBotClientWrapper.TgClient.DeleteMessageAsync(_options.TelegramChatId, relatedMessage.MessageId, cancellationToken);
            }
        }
    }
}