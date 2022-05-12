using LeagueActivityBot.Entities;
using MediatR;

namespace LeagueActivityBot.Notifications.OnGameStarted
{
    public class OnGameStartedNotification : INotification
    {
        public Summoner[] Summoners { get; }
        public long GameId { get; }
        public long QueueTypeId { get; }
        
        public OnGameStartedNotification(Summoner[] summoners, long gameId, long queueTypeId)
        {
            GameId = gameId;
            QueueTypeId = queueTypeId;
            Summoners = summoners;
        }
    }
}