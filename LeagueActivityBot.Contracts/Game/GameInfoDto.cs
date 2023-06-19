using System;

namespace LeagueActivityBot.Contracts.Game
{
    public class GameInfoDto
    {
        public int QueueId { get; set; }
        public long GameId { get; set; }
        public DateTime GameStartTime { get; set; }
        public long GameDurationInSeconds { get; set; }
        public GameParticipantDto[] Participants { get; set; }
    }
}