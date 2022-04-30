namespace LeagueActivityBot.Models
{
    public class CurrentGameInfo
    {
        public bool IsInGameNow { get; set; }
        public string GameType { get; set; }
        public long GameStartTime { get; set; }
        public long GameId { get; set; }
        public long GameQueueConfigId { get; set; }
        public string GameMode { get; set; }
        public GameParticipant[] Participants { get; set; }
    }

    public class GameParticipant
    {
        public string SummonerName { get; set; }
        public string SummonerId { get; set; }
    }
}