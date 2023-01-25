using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Notifications.OnGameEnded;
using LeagueActivityBot.Notifications.OnTeamGameEnded;
using LeagueActivityBot.Telegram.RecentMessages;
using Telegram.Bot;

namespace LeagueActivityBot.Telegram.Handlers
{
    public class OnTeamGameEndedNotificationHandler : INotificationHandler<OnTeamGameEndedNotification>
    {
        
        private readonly TelegramOptions _options;
        private readonly TelegramBotClientWrapper _telegramBotClientWrapper;
        private readonly OnTeamGameEndedMessageBuilder _messageBuilder;
        private readonly RecentMessageStore _recentMessageStore;

        public OnTeamGameEndedNotificationHandler(TelegramOptions options,  OnTeamGameEndedMessageBuilder messageBuilder, TelegramBotClientWrapper telegramBotClientWrapper, RecentMessageStore recentMessageStore)
        {
            _options = options;
            _messageBuilder = messageBuilder;
            _telegramBotClientWrapper = telegramBotClientWrapper;
            _recentMessageStore = recentMessageStore;
        }

        public async Task Handle(OnTeamGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var message = _messageBuilder.Build(notification);

            if (!string.IsNullOrEmpty(message))
            {
                await _telegramBotClientWrapper.SendAutoDeletableTextMessageAsync(_options.TelegramChatId, message, TimeSpan.FromHours(1), cancellationToken: cancellationToken);
                
                var relatedMessage = _recentMessageStore.Get(notification.MatchInfo.Info.GameId);
                if (relatedMessage != null)
                    await _telegramBotClientWrapper.TgClient.DeleteMessageAsync(_options.TelegramChatId, relatedMessage.MessageId, cancellationToken);
            }
        }
    }
}