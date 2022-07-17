using System;
using System.Linq;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Calendar.Models
{
    public class ClashInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationTime { get; set;}
        public DateTime EndTime { get; set;}

        public static ClashInfoDto CreateFromDomainModel(ClashInfo model)
        {
            var schedule = model.Schedule.FirstOrDefault(s => !s.Canceled);
            if (schedule == null) return null;

            var dto = new ClashInfoDto
            {
                Id = model.Id,
                Name = $"{model.Name} {model.SecondaryName}",
                RegistrationTime = schedule.RegistrationTime,
                EndTime = schedule.StartTime
            };

            return dto;
        }
    }
}