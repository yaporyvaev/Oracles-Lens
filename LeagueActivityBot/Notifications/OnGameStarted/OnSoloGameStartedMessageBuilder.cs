using LeagueActivityBot.Constants;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Utils;

namespace LeagueActivityBot.Notifications.OnGameStarted
{
    public class OnGameStartedMessageBuilder
    {
        public string Build(OnGameStartedNotification notification)
        {
            var names = SummonerNamesEnumerator.EnumerateSummoners(notification.Summoners);
            return $"{names} started {QueueTypeConstants.GetQueueTypeById(notification.QueueTypeId)}";
        } 
    }
}