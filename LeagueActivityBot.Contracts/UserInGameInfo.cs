using System;

namespace LeagueActivityBot.Contracts
{
    public class UserInGameInfo
    {
        public string SummonerName { get; set; }
        public long GameId { get; set; }
        public string GameMode { get; set; }
        public DateTime GameStartTime { get; set; }
        public string[] ParticipantNames { get; set; }
    }
}