using System.Text;
using LeagueActivityBot.Constants;

namespace LeagueActivityBot.Notifications.OnSoloGameStarted
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            var sb = new StringBuilder($"{notification.Summoner.GetName()} totally doesn't give a fuck about his friends and started playing solo {QueueType.GetQueueTypeById(notification.QueueTypeId)}");

            return sb.ToString();
        } 
    }
}
