using MediatR;

namespace LeagueActivityBot.Notifications
{
    public class OnSoloGameEndedNotification : INotification
    {
        public string SummonerName { get; }
        public long GameId { get; }
        
        public OnSoloGameEndedNotification(string summonerName, long gameId)
        {
            SummonerName = summonerName;
            GameId = gameId;
        }
    }
}