using System;
using System.Linq;
using System.Text;
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
                await tgClient.SendTextMessageAsync(new ChatId(_options.TelegramChatId), message, cancellationToken: cancellationToken,disableNotification: true);
            }
        }

        private async Task<string> BuildMessage(OnSoloGameEndedNotification notification)
        {
            var result = string.Empty;

            var matchInfo = await _riotClient.GetMatchInfo(notification.GameId);
            if (matchInfo == null) return result;
            
            var summonersStat = matchInfo.Info.Participants.First(p => p.SummonerName == notification.SummonerName);
            double teamDamage = matchInfo.Info.Participants
                .Where(p => p.TeamId == summonersStat.TeamId)
                .Sum(p => p.TotalDamageDealtToChampions);

            var damagePercentage = Math.Round(summonersStat.TotalDamageDealtToChampions / teamDamage * 100);

            var sb = new StringBuilder();

            sb.Append(
                summonersStat.Win
                ? $"{summonersStat.SummonerName} смог выиграть без друзей за {summonersStat.ChampionName}"
                : $"{summonersStat.SummonerName} обосрался за {summonersStat.ChampionName}"
            );

            sb.Append($" со счётом {summonersStat.Kills}/{summonersStat.Deaths}/{summonersStat.Assists} и нанёс {summonersStat.TotalDamageDealtToChampions} ({damagePercentage}%) урона!");
            if (damagePercentage < 20)
            {
                sb.Append(" Лох!");
            }
            
            return sb.ToString();
        }
        
    }
}