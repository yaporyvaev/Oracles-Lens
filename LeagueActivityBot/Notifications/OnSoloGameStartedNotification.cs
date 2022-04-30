using MediatR;

namespace LeagueActivityBot.Notifications
{
    public class OnSoloGameStartedNotification : INotification
    {
        public string SummonerName { get; }
        public long GameId { get; }
        public long QueueTypeId { get; }
        
        public OnSoloGameStartedNotification(string summonerName, long gameId, long queueTypeId)
        {
            SummonerName = summonerName;
            GameId = gameId;
            QueueTypeId = queueTypeId;
        }
    }
}