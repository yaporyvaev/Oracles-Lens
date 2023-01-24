using LeagueActivityBot.Constants;
using LeagueActivityBot.Helpers;

namespace LeagueActivityBot.Notifications.OnGameStarted
{
    public class OnGameStartedMessageBuilder
    {
        public string Build(OnTeamGameStartedNotification notification)
        {
            var names = SummonerNamesEnumerator.EnumerateSummoners(notification.Summoners);
            return $"{names} started {QueueTypeConstants.GetQueueTypeById(notification.QueueTypeId)}";
        } 
    }
}