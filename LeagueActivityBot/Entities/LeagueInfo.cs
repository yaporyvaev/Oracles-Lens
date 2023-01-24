using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities.Enums;

namespace LeagueActivityBot.Entities
{
    public class LeagueInfo : BaseEntity
    {
        public int SummonerId { get; set; }
        public LeagueType LeagueType { get; set; }
        public int Tier { get; set; }
        public int Rank { get; set; }       
        public int LeaguePoints { get; set; }
        
        public Summoner Summoner { get; set; }
    }
}