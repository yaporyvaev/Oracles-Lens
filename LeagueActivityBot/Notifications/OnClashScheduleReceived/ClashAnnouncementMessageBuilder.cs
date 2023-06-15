using System.Linq;

namespace LeagueActivityBot.Notifications.OnClashScheduleReceived
{
    public class ClashAnnouncementMessageBuilder
    {
        public string Build(ClashAnnouncementNotification notification)
        {
            var clash = notification.ClashInfos.FirstOrDefault();

            return $"<b>There is a clash today!</b>\r\n{clash!.Name} at {clash.Schedule.FirstOrDefault()!.RegistrationTime.ToLocalTime():hh:mm}";
        } 
    }
}