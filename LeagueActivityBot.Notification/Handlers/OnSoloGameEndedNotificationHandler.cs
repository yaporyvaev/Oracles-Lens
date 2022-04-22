using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Notifications;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LeagueActivityBot.Notification.Handlers
{
    public class OnGameEndedNotificationHandler : INotificationHandler<OnSoloGameEndedNotification>
    {
        private readonly NotificationOptions _options;
        private readonly IRiotClient _riotClient;

        public OnGameEndedNotificationHandler(NotificationOptions options, IRiotClient riotClient)
        {
            _options = options;
            _riotClient = riotClient;
        }

        public async Task Handle(OnSoloGameEndedNotification notification, CancellationToken cancellationToken)
        {
            var tgClient = new TelegramBotClient(_options.TelegramBotApiKey);

            var message = await BuildMessage(notification);
            if (!string.IsNullOrEmpty(message))
            {
                await tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken);
            }
        }

        private async Task<string> BuildMessage(OnSoloGameEndedNotification notification)
        {
            var result = string.Empty;

            var matchInfo = await _riotClient.GetMatchInfo(notification.GameId);
            if (matchInfo == null) return result;
            
            var stat = matchInfo.Info.Participants.FirstOrDefault(p => p.SummonerName == notification.SummonerName);
            if(stat == null)return result;

            result = stat.Win ? 
                $"{stat.SummonerName} смог выиграть без друзей за {stat.ChampionName} со счётом {stat.Kills}/{stat.Deaths}/{stat.Assists}" : 
                $"{stat.SummonerName} обосрался за {stat.ChampionName} со счётом {stat.Kills}/{stat.Deaths}/{stat.Assists}";
            
            return result;
        }
    }
}