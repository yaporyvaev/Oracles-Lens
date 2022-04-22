using MediatR;

namespace LeagueActivityBot.Notifications
{
    public class OnSoloGameStartedNotification : INotification
    {
        public string SummonerName { get; }
        public long GameId { get; }
        
        public OnSoloGameStartedNotification(string summonerName, long gameId)
        {
            SummonerName = summonerName;
            GameId = gameId;
        }
    }
}