using System;
using System.Collections.Generic;
using LeagueActivityBot.Abstractions;

namespace LeagueActivityBot.Entities
{
    public class GameInfo : BaseEntity
    {
        public long GameId { get; set; }
        public long QueueId { get; set; }
        public DateTime? GameStartTime { get; set; }
        public long? GameDurationInSeconds { get; set; }
        public bool GameEnded { get; set; }
        public bool IsProcessed { get; set; }
        public IList<GameParticipant> GameParticipants { get; set; }
    }
}