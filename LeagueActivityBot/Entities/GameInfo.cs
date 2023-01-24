using System;
using System.Collections.Generic;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities.Enums;

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

        public bool IsRankedGame => QueueId == (long) QueueType.RankedFlex || QueueId == (long) QueueType.RankedSoloDuo;
    }
}