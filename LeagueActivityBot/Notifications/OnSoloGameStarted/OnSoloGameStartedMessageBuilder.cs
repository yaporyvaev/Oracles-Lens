using System.Text;
using LeagueActivityBot.Constants;

namespace LeagueActivityBot.Notifications.OnSoloGameStarted
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            return $"<b><i>{notification.Summoner.Name}</i></b> started {QueueTypeConstants.GetQueueTypeById(notification.QueueTypeId)}.";
        } 
    }
}