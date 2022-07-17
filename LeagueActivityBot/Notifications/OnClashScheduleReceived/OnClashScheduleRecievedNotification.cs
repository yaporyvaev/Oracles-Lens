using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnClashScheduleReceived
{
    public class OnClashScheduleReceivedNotification : INotification
    {
        public OnClashScheduleReceivedNotification(ClashInfo[] clashInfos)
        {
            ClashInfos = clashInfos;
        }

        public ClashInfo[] ClashInfos { get; }
    }
}