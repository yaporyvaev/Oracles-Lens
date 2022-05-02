using LeagueActivityBot.Abstractions;

namespace LeagueActivityBot.Entities
{
    public class Summoner : BaseEntity
    {
        public string SummonerId { get; set; }
        public string AccountId { get; set; }
        public string Puuid { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public int Tier { get; set; }
        public int Rank { get; set; }       
        public int LeaguePoints { get; set; }
    }
}