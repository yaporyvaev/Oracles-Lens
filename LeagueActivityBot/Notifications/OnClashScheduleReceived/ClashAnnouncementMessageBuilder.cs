using System.Linq;

namespace LeagueActivityBot.Notifications.OnClashScheduleReceived
{
    public static class ClashAnnouncementMessageBuilder
    {
        public static string Build(ClashAnnouncementNotification notification)
        {
            var clash = notification.ClashInfos.FirstOrDefault();
            return $"<b>There is a clash today at {clash!.Schedule.FirstOrDefault()!.RegistrationTime.ToLocalTime():HH:mm}!</b>";
        } 
    }
}