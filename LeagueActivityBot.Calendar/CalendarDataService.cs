using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueActivityBot.Calendar.Integration;
using LeagueActivityBot.Calendar.Integration.Models;
using LeagueActivityBot.Calendar.Models;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Calendar
{
    public class CalendarDataService
    {
        private readonly CalendarHttpClient _calendarHttpClient;

        public CalendarDataService(CalendarHttpClient calendarHttpClient)
        {
            _calendarHttpClient = calendarHttpClient;
        }

        public async Task AddEvents(IEnumerable<ClashInfo> clashInfos)
        {
            var clashInfoDtos = clashInfos
                .Select(ClashInfoDto.CreateFromDomainModel)
                .Where(s => s != null)
                .OrderBy(s => s.RegistrationTime)
                .Take(2);

            var requestModels = clashInfoDtos.Select(c => new AddEventRequestModel
            {
                Summary = "Clash",
                EventId = GenerateSourceId(c),
                EndDate = c.EndTime,
                StartDate = c.RegistrationTime
            });

            await _calendarHttpClient.AddEvent(requestModels);
        }

        private string GenerateSourceId(ClashInfoDto clashInfo)
        {
            var bytes = Encoding.UTF8.GetBytes($"{Constants.ClashSourceIdPrefix}:{clashInfo.Id}");
            return Convert.ToBase64String(bytes);
        }
    }
}