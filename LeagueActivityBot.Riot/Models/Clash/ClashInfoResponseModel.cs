namespace LeagueActivityBot.Riot.Models.Clash
{
    public class ClashInfoResponseModel
    {
        public int Id { get; set; }
        public string NameKey { get; set; }
        public string NameKeySecondary { get; set; }
        public ClashScheduleResponseModel[] Schedule { get; set; }
    }
}