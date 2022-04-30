using System.Text;
using LeagueActivityBot.Constatnts;

namespace LeagueActivityBot.Notifications.MessageBuilders
{
    public class OnSoloGameStartedMessageBuilder
    {
        public string Build(OnSoloGameStartedNotification notification)
        {
            var sb = new StringBuilder($"{notification.SummonerName} крыса, начал играть {QueueType.GetQueueTypeById(notification.QueueTypeId)}");
            return sb.ToString();
        } 
    }
}