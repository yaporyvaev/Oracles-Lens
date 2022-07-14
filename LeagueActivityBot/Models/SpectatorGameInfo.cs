namespace LeagueActivityBot.Models
{
    public class SpectatorGameInfo
    {
        public bool IsInGameNow { get; set; }
        public long GameId { get; set; }
        public long GameQueueConfigId { get; set; }
        public SpectatorGameParticipant[] Participants { get; set; }
    }

    public class SpectatorGameParticipant
    {
        public string SummonerName { get; set; }
        public string SummonerId { get; set; }
    }
}