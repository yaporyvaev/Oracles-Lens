using System.Threading;
using System.Threading.Tasks;
using LeagueActivityBot.Calendar.Integration;
using LeagueActivityBot.Notifications.OnClashScheduleReceived;
using MediatR;

namespace LeagueActivityBot.Calendar
{
    public class OnClashScheduleReceivedNotificationHandler : INotificationHandler<OnClashScheduleReceivedNotification>
    {
        private readonly CalendarDataService _calendarDataService;

        public OnClashScheduleReceivedNotificationHandler(CalendarHttpClient calendarHttpClient, CalendarDataService calendarDataService)
        {
            _calendarDataService = calendarDataService;
        }

        public async Task Handle(OnClashScheduleReceivedNotification notification, CancellationToken cancellationToken)
        {
            await _calendarDataService.AddEvents(notification.ClashInfos);
        }
    }
}