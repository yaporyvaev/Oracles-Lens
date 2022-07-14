using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnSoloGameEnded
{
    public class OnSoloGameEndedNotification : INotification
    {
        public Summoner Summoner { get; }
        public MatchInfo MatchInfo { get; }
        
        public OnSoloGameEndedNotification(Summoner summoner, MatchInfo matchInfo)
        {
            Summoner = summoner;
            MatchInfo = matchInfo;
        }
    }
}