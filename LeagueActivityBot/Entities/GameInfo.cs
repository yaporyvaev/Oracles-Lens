using System;
using LeagueActivityBot.Abstractions;

namespace LeagueActivityBot.Entities
{
    public class GameInfo : BaseEntity
    {
        public long GameId { get; set; }
        public long QueueId { get; set; }
        public DateTime GameStartTime { get; set; }
        public string SummonerNamesJson { get; set; }
        public bool IsProcessed { get; set; }
    }
}