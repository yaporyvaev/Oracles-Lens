using System.Collections.Generic;
using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnClashScheduleReceived
{
    public class ClashAnnouncementNotification : INotification
    {
        public ClashAnnouncementNotification(IEnumerable<ClashInfo> clashInfos)
        {
            ClashInfos = clashInfos;
        }

        public IEnumerable<ClashInfo> ClashInfos { get; }
    }
}