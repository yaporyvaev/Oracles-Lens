using System.Collections.Generic;
using LeagueActivityBot.Abstractions;

namespace LeagueActivityBot.Entities
{
    public class Summoner : BaseEntity
    {
        public string SummonerId { get; set; }
        public string AccountId { get; set; }
        public string Puuid { get; set; }
        public string Name { get; set; }

        public IList<GameParticipant> GameParticipants { get; set; }
        public IList<LeagueInfo> LeagueInfos { get; set; }
    }
}