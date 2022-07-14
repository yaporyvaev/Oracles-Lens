namespace LeagueActivityBot.Models
{
    public class SummonerStatistic
    {
        public string SummonerName { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        
        public double WinRate => (double) Wins / (Wins + Loses) * 100;
    }
}