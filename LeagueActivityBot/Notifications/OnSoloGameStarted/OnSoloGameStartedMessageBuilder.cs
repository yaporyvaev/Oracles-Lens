using System.Text;
using LeagueActivityBot.Constants;

namespace LeagueActivityBot.Notifications.OnSoloGameStarted
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            return $"{notification.Summoner.Name} started {QueueTypeConstants.GetQueueTypeById(notification.QueueTypeId)}.";
        } 
    }
}