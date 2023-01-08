using System;
using System.Collections.Generic;
using System.Linq;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Services
{
    public class ClashService
    {
        public static IEnumerable<ClashInfo> GetClashesForADay(IEnumerable<ClashInfo> clashes, DateTime day)
        {
            var result = new List<ClashInfo>();
            if (clashes == null) return result;
            
            foreach (var clash in clashes)
            {
                var todaySchedules = clash.Schedule
                    .Where(clashSchedule => !clashSchedule.Canceled && clashSchedule.RegistrationTime.Date == day.Date)
                    .OrderBy(s => s.RegistrationTime)
                    .ToArray();

                if (todaySchedules.Any())
                {
                    clash.Schedule = todaySchedules;
                    result.Add(clash);
                }
            }

            return result;
        }
    }
}