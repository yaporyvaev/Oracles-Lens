using LeagueActivityBot.Contracts.Summoners;

namespace LeagueActivityBot.Contracts.Game
{
    public class GetGameInfoResponse
    {
        public GameInfoDto GameInfo { get; set; }
        public SummonerDto[] RegisteredSummoners { get; set; }
        public string DataApiVersion { get; set; }
    }
}