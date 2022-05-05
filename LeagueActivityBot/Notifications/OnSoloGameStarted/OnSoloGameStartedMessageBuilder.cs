using System.Text;
using LeagueActivityBot.Constants;

namespace LeagueActivityBot.Notifications.OnSoloGameStarted
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            var summerName = !string.IsNullOrEmpty(notification.Summoner.RealName) ? notification.Summoner.RealName : notification.Summoner.Name;
            var sb = new StringBuilder($"{summerName} крыса, начал играть {QueueType.GetQueueTypeById(notification.QueueTypeId)}");

            if (!string.IsNullOrEmpty(notification.Summoner.RealName))
            {
                sb.Append($" (as {notification.Summoner.Name})");
            }
            
            return sb.ToString();
        } 
    }
}