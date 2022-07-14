using JetBrains.Annotations;
using LeagueActivityBot.Abstractions;

namespace LeagueActivityBot.Entities
{
    public class GameParticipant : BaseEntity
    {
        public int SummonerId { get; set; }
        public Summoner Summoner { get; set; }
        
        public int GameInfoId { get; set; }
        public GameInfo GameInfo { get; set; }
        
        public bool? Win { get; set; }
        public int? Kills { get; set; }
        public int? Deaths { get; set; }
        public int? Assists { get; set; }
        public int? CreepScore { get; set; }
        public int? VisionScore { get; set; }
        public int? ChampionId { get; set; }
        [CanBeNull] public string ChampionName { get; set; }
        public int? DetectorWardsPlaced { get; set; }
        public bool? FirstBloodKill { get; set; }
        public int? PentaKills { get; set; }
    }
}