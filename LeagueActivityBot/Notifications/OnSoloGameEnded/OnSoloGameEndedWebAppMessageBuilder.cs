using System.Linq;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using MediatR;

namespace LeagueActivityBot.Notifications.OnSoloGameEnded
{
    [UsedImplicitly]
    public class OnSoloGameEndedWebAppMessageBuilder : INotificationBuilder
    {
        public string Build(INotification baseNotification)
        {
            var notification = (OnSoloGameEndedNotification)baseNotification;
            
            var matchInfo = notification.MatchInfo;
            if (matchInfo == null) return string.Empty;
            
            var summonersStat = matchInfo.Info.Participants.First(p => p.SummonerName == notification.Summoner.Name);
            var summoner = notification.Summoner;

            var matchResult = BaseEndGameMessageBuilder.GetMatchResult(summonersStat);
            var result = $"<a href=\"{notification.WebAppUrl}?startapp={matchInfo.Info.GameId}&startApp={matchInfo.Info.GameId}\"><b><i>{summoner.Name}</i></b> {matchResult} {QueueTypeConstants.GetQueueTypeById(notification.MatchInfo.Info.QueueId)}</a>\n";

            foreach (var participant in matchInfo.Info.Participants.OrderByDescending(p => p.Score))
            {
                if (participant.Score == null) break;
                result += $"\n{participant.ChampionName} - {participant.Score}";
            }
            
            return result;
        }
    }
}