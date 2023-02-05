namespace LeagueActivityBot.Models
{
    public class EndGameLeagueDelta
    {
        public string SummonerId { get; set; }
        public Entities.LeagueInfo CurrentLeagueInfo { get; set; }
        public bool IsAnyDelta { get; set; } = true;
        public bool LeagueUpdated { get; set; }
        public int LeaguePointsDelta { get; set; }
    }
}