using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnSoloGameEnded
{
    public class OnSoloGameEndedNotification : INotification
    {
        public Summoner Summoner { get; }
        public MatchInfo MatchInfo { get; }
        public EndGameLeagueDelta LeagueDelta { get; }
        public string WebAppUrl { get; set; }
        public OnSoloGameEndedNotification(Summoner summoner, MatchInfo matchInfo, EndGameLeagueDelta leagueDelta)
        {
            Summoner = summoner;
            MatchInfo = matchInfo;
            LeagueDelta = leagueDelta;
        }
    }
}