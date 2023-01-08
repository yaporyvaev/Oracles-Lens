using System.Text;

namespace LeagueActivityBot.Notifications.OnClashScheduleReceived
{
    public class ClashAnnouncementMessageBuilder
    {
        public string Build(ClashAnnouncementNotification notification)
        {
            var sb = new StringBuilder("There is clash today!");
            foreach (var clash in notification.ClashInfos)
            {
                sb.AppendLine($"{clash.Name} {clash.SecondaryName} at ");
                
                var notFirst = false;
                foreach (var schedule in clash.Schedule)
                {
                    if(notFirst) sb.Append(" ,");
                    sb.Append(schedule.RegistrationTime.ToString("hh:mm"));
                    notFirst = true;
                }
            }

            return sb.ToString();
        } 
    }
}