using System.Collections.Generic;

namespace LeagueActivityBot.Contracts
{
    public class MatchResult
    {
        public List<Score> Scores { get; set; }
    }

    public class Score
    {
        public string SummonerName { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public string ChampionName { get; set; }
        
        public bool Win { get; set; }
    }
}