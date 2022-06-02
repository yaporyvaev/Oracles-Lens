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

            var result = $"{names} начали играть {QueueType.GetQueueTypeById(notification.QueueTypeId)}";
            return result;
        } 
    }
}