namespace LeagueActivityBot.Models
{
    public class MatchInfo
    {
        public Info Info { get; set; }
    }

    public class Info
    {
        public MatchParticipant[] Participants { get; set; }
    }

    public class MatchParticipant
    {
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        
        public string Puuid { get; set; }
        public string SummonerId { get; set; }
        public string SummonerName { get; set; }
        
        public string ChampionName { get; set; }
        public int TeamId { get; set; }
        public bool Win { get; set; }
    }
}