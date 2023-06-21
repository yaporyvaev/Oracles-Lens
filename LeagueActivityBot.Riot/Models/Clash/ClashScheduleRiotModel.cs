namespace LeagueActivityBot.Riot.Models.Clash
{
    public class ClashScheduleRiotModel
    {
        public int Id { get; set;}
        public long RegistrationTime { get; set;}
        public long StartTime { get; set;}
        public bool Canceled { get; set;}
    }
}