using System.Collections.Generic;

namespace LeagueActivityBot.Models
{
    public class LeagueInfo
    {
        public string LeagueId { get; set; }       
        public string QueueType { get; set; }       
        public string Tier { get; set; }       
        public string Rank { get; set; }       
        public int LeaguePoints { get; set; }       
        public int Wins { get; set; }       
        public int Losses { get; set; }

        public int GetTierIntegerRepresentation()
        {
            return Tier switch
            {
                "IRON" => 1,
                "BRONZE" => 2,
                "SILVER" => 3,
                "GOLD" => 4,
                "PLATINUM" => 5,
                "DIAMOND" => 6,
                "MASTER" => 7,
                "GRANDMASTER" => 8,
                _ => 0
            };
        }
        
        public int GetRankIntegerRepresentation()
        {
            return Rank switch
            {
                "I" => 1,
                "II" => 2,
                "III" => 3,
                "IV" => 4,
                _ => 0
            };
        }
    }
}