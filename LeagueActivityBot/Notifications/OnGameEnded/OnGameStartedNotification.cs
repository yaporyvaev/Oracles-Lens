using LeagueActivityBot.Entities;
using MediatR;

namespace LeagueActivityBot.Notifications.OnGameEnded
{
    public class OnGameEndedNotification : INotification
    {
        public Summoner[] Summoners { get; }
        public long GameId { get; }
        
        public OnGameEndedNotification(Summoner[] summoners, long gameId)
        {
            GameId = gameId;
            Summoners = summoners;
        }
    }
}