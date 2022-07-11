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
            
            var sb = new StringBuilder($"<b><i>{_summoner.Name}</i></b> {GetAction()} {GetChampion()}.\n{_summonersStat.GetScore()}, {GetDamage()}.");

            if (_matchInfo.Info.QueueId != (int)QueueType.ARAM)
            {
                sb.Append($" {_summonersStat.GetCreepScore()}, {_summonersStat.GetVisionScore()}.");
            }
            
            if (_matchInfo.Info.QueueId == (int)QueueType.RankedSoloDuo)
            {
                sb.Append($"\n{await GetRankedStat()}");
            }
            
            return sb.ToString();
        }
        
        private string GetAction()
        {
            if (_summonersStat.Win) return "won";
            if (_summonersStat.GameEndedInEarlySurrender) return "FFed 15";
            if (_summonersStat.GameEndedInSurrender) return "FFed";
                
            return "lost";
        }
        private string GetChampion() => $"on {_summonersStat.ChampionName}";

        private string GetDamage() => _summonersStat.GetDamage(_matchInfo.Info.GetTeamDamage(_summonersStat.TeamId));

        private async Task<string> GetRankedStat()
        {
            var currentLeague = (await _riotClient.GetLeagueInfo(_summoner.SummonerId)).FirstOrDefault(l =>
                l.QueueType == QueueTypeConstants.RankedSolo);
            if (currentLeague == null) return string.Empty;
                
            var sb = new StringBuilder();
            if (currentLeague.GetTierIntegerRepresentation() == _summoner.Tier && currentLeague.GetRankIntegerRepresentation() == _summoner.Rank)
            {
                if (currentLeague.LeaguePoints != _summoner.LeaguePoints)
                {
                    if (_summonersStat.Win) sb.Append("+");
                    sb.Append($"{currentLeague.LeaguePoints - _summoner.LeaguePoints} LP. ");
                }

                sb.Append($"Current rank is {currentLeague.Tier} {currentLeague.Rank}, {currentLeague.LeaguePoints} LP."); 
            }
            else
            {
                sb.Append(_summonersStat.Win
                    ? $"Promoted to {currentLeague.Tier} {currentLeague.Rank}!"
                    : $"Demoted to {currentLeague.Tier} {currentLeague.Rank}.");
            }

            return sb.ToString();
        }
    }
}