using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Notifications.OnSoloGameEnded
{
    public class OnSoloGameEndedMessageBuilder
    {
        private readonly IRiotClient _riotClient;
        private MatchInfo _matchInfo;
        private Summoner _summoner;
        private MatchParticipant _summonersStat;
        
        public OnSoloGameEndedMessageBuilder(IRiotClient riotClient)
        {
            _riotClient = riotClient;
        }

        public async Task<string> Build(OnSoloGameEndedNotification notification)
        {
            _matchInfo = await _riotClient.GetMatchInfo(notification.GameId);
            if (_matchInfo == null) return string.Empty;
            
            _summonersStat = _matchInfo.Info.Participants.First(p => p.SummonerName == notification.Summoner.Name);
            _summoner = notification.Summoner;
            
            var sb = new StringBuilder($"{GetActor()} {GetAction()} {GetChampion()}, {_summonersStat.GetScore()}, {GetDamage()}.\n {await GetRankedStat()}\n {GetPersonal()}");
            return sb.ToString();
        }
        
        private string GetActor() => $"{_summoner.GetName()}";
        private string GetAction()
        {
            if (_summonersStat.Win) return "has no one to share the joy of victory";
            if (_summonersStat.GameEndedInEarlySurrender) return "tilted and gave up";
            if (_summonersStat.GameEndedInSurrender) return "decided to save 1 minute and gave up";
                
            return "lost";
        }
        private string GetChampion() => $"by {_summonersStat.ChampionName}. Shame on you, douchebag!";
        private string GetDamage()
        {
            return _summonersStat.GetDamage(_matchInfo.Info.GetTeamDamage(_summonersStat.TeamId));
        }
        private async Task<string> GetRankedStat()
        {
            if (_matchInfo.Info.QueueId != 420) return string.Empty;
            
            var currentLeague = (await _riotClient.GetLeagueInfo(_summoner.SummonerId)).FirstOrDefault(l =>
                l.QueueType == QueueType.RankedSolo);
            if (currentLeague == null) return string.Empty;
                
            var sb = new StringBuilder();
            if (currentLeague.GetTierIntegerRepresentation() == _summoner.Tier && currentLeague.GetRankIntegerRepresentation() == _summoner.Rank)
            {
                if (currentLeague.LeaguePoints != _summoner.LeaguePoints)
                {
                    if (_summonersStat.Win) sb.Append("+");
                    sb.Append($"{currentLeague.LeaguePoints - _summoner.LeaguePoints} LP.");
                }

                sb.Append($"Current rank {currentLeague.Tier} {currentLeague.Rank}, {currentLeague.LeaguePoints} LP"); 
            }
            else
            {
                sb.Append(_summonersStat.Win
                    ? $"is a good boy, now he's {currentLeague.Tier} {currentLeague.Rank}. Great job!"
                    : $"demoted to {currentLeague.Tier} {currentLeague.Rank} for shamefully defeat. Looser!");
            }

            sb.Append(".");
            return sb.ToString();
        }
        
        private string GetPersonal()
        {
            if (!_summonersStat.Win && _summonersStat.SummonerName == "AidenGrimes") return "but still just a pretty guy!";
            
            return string.Empty;
        }
    }
}