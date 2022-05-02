using System.Text;
using LeagueActivityBot.Constatnts;

namespace LeagueActivityBot.Notifications.MessageBuilders
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            var summerName = !string.IsNullOrEmpty(notification.Summoner.RealName) ? notification.Summoner.RealName : notification.Summoner.Name;
            var sb = new StringBuilder($"{summerName} крыса, начал играть {QueueType.GetQueueTypeById(notification.QueueTypeId)}");
            return sb.ToString();
        } 
    }
}