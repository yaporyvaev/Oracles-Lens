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

        public static string GetRankStringRepresentation(int rank)
        {
            return rank switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                4 => "IV",
                _ => ""
            };
        }

        public static string GetTierStringRepresentation(int tier)
        {
            return tier switch
            {
                1 => "Iron",
                2 => "Bronze" ,
                3 => "Silver",
                4 => "Gold",
                5 => "Platinum",
                6 => "Diamond",
                7 => "Master",
                8 => "Grandmaster",
                _ => ""
            };
        }
    }
}