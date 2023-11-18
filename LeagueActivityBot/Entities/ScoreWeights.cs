using LeagueActivityBot.Abstractions;

namespace LeagueActivityBot.Entities
{
    public class ScoreWeights : BaseEntity
    {
        public double Kda { get; set; }
        public double Level { get; set; }
        public double Gold { get; set; }
        public double CcTime { get; set; }
        public double DmgToChampions { get; set; }
        public double DmgTaken { get; set; }
        public double DmgMitigated { get; set; }
        public double DmgHealed { get; set; }
        public double DmgShielded { get; set; }
    }
}