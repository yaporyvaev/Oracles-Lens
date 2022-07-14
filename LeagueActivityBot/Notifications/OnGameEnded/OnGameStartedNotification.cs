using System.Collections.Generic;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnGameEnded
{
    public class OnGameEndedNotification : INotification
    {
        public IEnumerable<Summoner> Summoners { get; }
        
        public MatchInfo MatchInfo { get; }
        
        public OnGameEndedNotification(IEnumerable<Summoner> summoners, MatchInfo matchInfo)
        {
            MatchInfo = matchInfo;
            Summoners = summoners;
        }
    }
}