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
            
            var sb = new StringBuilder($"{GetActor()} {GetAction()} {GetChampion()}, {GetScore()}, {GetDamage()}.\n {await GetRankedStat()}\n {GetPersonal()}");
            return sb.ToString();
        }
        
        private string GetActor() => $"{(!string.IsNullOrEmpty(_summoner.RealName) ? _summoner.RealName : _summoner.Name)}";
        private string GetAction()
        {
            if (_summonersStat.Win) return "победил";
            if (_summonersStat.GameEndedInEarlySurrender) return "написал фф на 15))))00";
            if (_summonersStat.GameEndedInSurrender) return "сдался";
                
            return "проиграл";
        }
        private string GetChampion() => $"за {_summonersStat.ChampionName}";
        private string GetScore() => $"KDA {_summonersStat.Kills}/{_summonersStat.Deaths}/{_summonersStat.Assists}";
        private string GetDamage()
        {
            double teamDamage = _matchInfo.Info.Participants
                .Where(p => p.TeamId == _summonersStat.TeamId)
                .Sum(p => p.TotalDamageDealtToChampions);
            var damagePercentage = Math.Round(_summonersStat.TotalDamageDealtToChampions / teamDamage * 100);

            var sb = new StringBuilder($"{_summonersStat.TotalDamageDealtToChampions.ToString($"#,#")} ({damagePercentage}%) урона");
            return sb.ToString();
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
                    sb.Append($"{currentLeague.LeaguePoints - _summoner.LeaguePoints} LP. ");
                }

                sb.Append($"Текущий ранг {currentLeague.Tier} {currentLeague.Rank}, {currentLeague.LeaguePoints} LP"); 
            }
            else
            {
                sb.Append(_summonersStat.Win
                    ? $"Наш мальчик поднялся, теперь он {currentLeague.Tier} {currentLeague.Rank}!"
                    : $"Даунранкед до {currentLeague.Tier} {currentLeague.Rank} =(");
            }

            sb.Append(".");
            return sb.ToString();
        }
        
        private string GetPersonal()
        {
            if (!_summonersStat.Win && _summonersStat.SummonerName == "AidenGrimes") return "Но все равно молодец!";
            
            return string.Empty;
        }
    }
}