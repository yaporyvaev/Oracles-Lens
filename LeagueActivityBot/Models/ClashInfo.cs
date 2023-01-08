using System;

namespace LeagueActivityBot.Models
{
    public class ClashInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SecondaryName { get; set; }
        public ClashSchedule[] Schedule { get; set; }
    }

    public class ClashSchedule
    {
        public int Id { get; set;}
        public DateTime RegistrationTime { get; set;}
        public DateTime StartTime { get; set;}
        public bool Canceled { get; set;}
    }
}