using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using LeagueActivityBot.Notifications.OnGameEnded;

namespace LeagueActivityBot.Notifications.OnTeamGameEnded
{
    public class OnTeamGameEndedMessageBuilder
    {
        private MatchInfo _matchInfo;
        private Summoner[] _summoners;
        private Dictionary<string, EndGameLeagueDelta> _leagueDeltas;

        public string Build(OnTeamGameEndedNotification notification)
        {
            _matchInfo = notification.MatchInfo;
            if (_matchInfo == null) return string.Empty;
            _summoners = notification.Summoners.ToArray();
            _leagueDeltas = notification.LeagueDelta;

            var matchResult = BaseEndGameMessageBuilder.GetMatchResult(_matchInfo.Info.Participants.First(p => p.SummonerName == _summoners.First().Name));
            var sb = new StringBuilder($"Team {matchResult} {QueueTypeConstants.GetQueueTypeById(notification.MatchInfo.Info.QueueId)}\n\n{GetStats()}");
            return sb.ToString();
        }

        private string GetStats()
        {
            var sb = new StringBuilder();

            var team1Damage = _matchInfo.Info.GetTeamDamage(100);
            var team2Damage = _matchInfo.Info.GetTeamDamage(200);

            foreach (var summoner in _summoners)
            {
                var stat = _matchInfo.Info.Participants.First(p => p.SummonerName == summoner.Name);
                sb.Append($"<b><i>{summoner.Name}</i></b> on {stat.ChampionName}.\n{stat.GetScore()}, {stat.GetDamage(stat.TeamId == 100? team1Damage : team2Damage)}.");
                
                if (_matchInfo.Info.QueueId != (int)QueueType.ARAM)
                {
                    sb.Append($" {stat.GetCreepScore()}, {stat.GetVisionScore()}.");
                }
                
                if (_matchInfo.Info.QueueId == (int)QueueType.RankedSoloDuo || _matchInfo.Info.QueueId == (int)QueueType.RankedFlex)
                {
                    sb.Append($"\n{BaseEndGameMessageBuilder.GetRankedStat(_leagueDeltas[summoner.SummonerId], stat.Win)}");
                }
                
                sb.Append("\n\n");
            }

            return sb.ToString();
        }
    }
}