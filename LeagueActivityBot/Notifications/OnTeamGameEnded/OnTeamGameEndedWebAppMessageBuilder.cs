using System.Linq;
using System.Text;
using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using MediatR;

namespace LeagueActivityBot.Notifications.OnTeamGameEnded
{
    [UsedImplicitly]
    public class OnTeamGameEndedWebAppMessageBuilder : INotificationBuilder
    {
        public string Build(INotification baseNotification)
        {
            var notification = (OnTeamGameEndedNotification)baseNotification;

            var matchInfo = notification.MatchInfo;
            if (matchInfo == null) return string.Empty;
            
            var summoners = notification.Summoners.ToArray();
            var matchResult = BaseEndGameMessageBuilder.GetMatchResult(matchInfo.Info.Participants.First(p => p.SummonerName == summoners.First().Name));
            
            var sb = new StringBuilder(
                $"<a href=\"{notification.WebAppUrl}?startapp={matchInfo.Info.GameId}&startApp={matchInfo.Info.GameId}\">Team {matchResult} {QueueTypeConstants.GetQueueTypeById(notification.MatchInfo.Info.QueueId)}</a>");
            
            return sb.ToString();
        }

    }
}