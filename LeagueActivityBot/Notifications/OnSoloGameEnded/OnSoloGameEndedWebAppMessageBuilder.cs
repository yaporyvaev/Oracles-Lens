using System.Linq;
using System.Text;
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
            var sb = new StringBuilder(
                $"<a href=\"{notification.WebAppUrl}?startapp={matchInfo.Info.GameId}&startApp={matchInfo.Info.GameId}\"><b><i>{summoner.Name}</i></b> {matchResult} {QueueTypeConstants.GetQueueTypeById(notification.MatchInfo.Info.QueueId)}</a>");
            
            return sb.ToString();
        }
    }
}