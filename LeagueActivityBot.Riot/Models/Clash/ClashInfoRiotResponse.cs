namespace LeagueActivityBot.Riot.Models.Clash
{
    public class ClashInfoRiotResponse
    {
        public int Id { get; set; }
        public string NameKey { get; set; }
        public string NameKeySecondary { get; set; }
        public ClashScheduleRiotModel[] Schedule { get; set; }
    }
}