using System.Text;
using LeagueActivityBot.Constants;

namespace LeagueActivityBot.Notifications.OnSoloGameStarted
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            var sb = new StringBuilder($"{notification.Summoner.GetName()} крыса, начал играть {QueueType.GetQueueTypeById(notification.QueueTypeId)}");

            return sb.ToString();
        } 
    }
}