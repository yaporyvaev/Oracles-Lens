using System.Linq;
using System.Text;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using MediatR;

namespace LeagueActivityBot.Notifications.OnSoloGameEnded
{
    public class OnSoloGameEndedTextMessageBuilder : INotificationBuilder
    {
        private MatchInfo _matchInfo;
        private Summoner _summoner;
        private MatchParticipant _summonersStat;

        public string Build(INotification baseNotification)
        {
            var notification = (OnSoloGameEndedNotification)baseNotification;
            _matchInfo = notification.MatchInfo;
            if (_matchInfo == null) return string.Empty;
            
            _summonersStat = _matchInfo.Info.Participants.First(p => p.SummonerName == notification.Summoner.Name);
            _summoner = notification.Summoner;

            var matchResult = BaseEndGameMessageBuilder.GetMatchResult(_summonersStat);
            var sb = new StringBuilder($"<b><i>{_summoner.Name}</i></b> {matchResult} {QueueTypeConstants.GetQueueTypeById(notification.MatchInfo.Info.QueueId)} {GetChampion()}.\n{_summonersStat.GetScore()}, {GetDamage()}.");

            if (_matchInfo.Info.QueueId != (int)QueueType.ARAM)
            {
                sb.Append($" {_summonersStat.GetCreepScore()}, {_summonersStat.GetVisionScore()}.");
            }
            
            if (_matchInfo.Info.QueueId == (int)QueueType.RankedSoloDuo || _matchInfo.Info.QueueId == (int)QueueType.RankedFlex)
            {
                sb.Append($"\n{BaseEndGameMessageBuilder.GetRankedStat(notification.LeagueDelta, _summonersStat.Win)}");
            }
            
            return sb.ToString();
        }
        
        private string GetChampion() => $"on {_summonersStat.ChampionName}";
        private string GetDamage() => _summonersStat.GetDamage(_matchInfo.Info.GetTeamDamage(_summonersStat.TeamId));
    }
}