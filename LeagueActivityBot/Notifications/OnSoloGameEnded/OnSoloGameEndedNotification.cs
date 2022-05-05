using LeagueActivityBot.Entities;
using MediatR;

namespace LeagueActivityBot.Notifications.OnSoloGameEnded
{
    public class OnSoloGameEndedNotification : INotification
    {
        public Summoner Summoner { get; }
        public long GameId { get; }
        
        public OnSoloGameEndedNotification(Summoner summoner, long gameId)
        {
            Summoner = summoner;
            GameId = gameId;
        }
    }
}