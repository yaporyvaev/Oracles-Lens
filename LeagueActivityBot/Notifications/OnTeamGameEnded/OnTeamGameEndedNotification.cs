using System.Collections.Generic;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnTeamGameEnded
{
    public class OnTeamGameEndedNotification : INotification
    {
        public IEnumerable<Summoner> Summoners { get; }
        public MatchInfo MatchInfo { get; }
        public Dictionary<string, EndGameLeagueDelta> LeagueDelta { get; }
        public string WebAppUrl { get; set; }

        public OnTeamGameEndedNotification(IEnumerable<Summoner> summoners, MatchInfo matchInfo, Dictionary<string, EndGameLeagueDelta> leagueDelta)
        {
            MatchInfo = matchInfo;
            LeagueDelta = leagueDelta;
            Summoners = summoners;
        }
    }
}