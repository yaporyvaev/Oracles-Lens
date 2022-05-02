using LeagueActivityBot.Entities;
using MediatR;

namespace LeagueActivityBot.Notifications
{
    public class OnSoloGameStartedNotification : INotification
    {
        public Summoner Summoner { get; }
        public long GameId { get; }
        public long QueueTypeId { get; }
        
        public OnSoloGameStartedNotification(Summoner summoner, long gameId, long queueTypeId)
        {
            GameId = gameId;
            QueueTypeId = queueTypeId;
            Summoner = summoner;
        }
    }
}