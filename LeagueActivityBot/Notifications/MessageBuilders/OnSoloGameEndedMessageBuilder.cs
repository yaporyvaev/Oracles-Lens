using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;

namespace LeagueActivityBot.Notifications.MessageBuilders
{
    public class OnSoloGameEndedMessageBuilder
    {
        private readonly IRiotClient _riotClient;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly Random _randomizer;
        private MatchInfo _matchInfo;
        private MatchParticipant _participantStat;
        private string _summonerName;

        private readonly Dictionary<int, string> _winPhrasesDictionary = new Dictionary<int, string>
        {
            {0, "смог выиграть без друзей"},
            {1, "фак ёр мазер вери велл"}
        };
            
        private readonly Dictionary<int, string> _losePhrasesDictionary = new Dictionary<int, string>
        {
            {0, "обосрался"},
            {1, "проиграл"}
        };
        
        public OnSoloGameEndedMessageBuilder(IRiotClient riotClient, IRepository<Summoner> summonerRepository)
        {
            _riotClient = riotClient;
            _summonerRepository = summonerRepository;
            _randomizer = new Random();
        }

        public async Task<string> Build(OnSoloGameEndedNotification notification)
        {
            _matchInfo = await _riotClient.GetMatchInfo(notification.GameId);
            if (_matchInfo == null) return string.Empty;
            
            _participantStat = _matchInfo.Info.Participants.First(p => p.SummonerName == notification.SummonerName);
            _summonerName = notification.SummonerName;
            
            var sb = new StringBuilder($"{GetActor()} {GetAction()} {GetChampion()} {GetScore()} {GetDamage()} {await GetRankedStat()} {GetPersonal()}");
            return sb.ToString();
        }
        
        private string GetActor() => $"{_participantStat.SummonerName}";
        private string GetChampion() => $"за {_participantStat.ChampionName}";
        private string GetAction()
        {
            if (_participantStat.Win) return _winPhrasesDictionary[_randomizer.Next(0, _winPhrasesDictionary.Count)];

            if (_participantStat.GameEndedInEarlySurrender) return "написал фф на 15))))00";
            if (_participantStat.GameEndedInSurrender) return "сдался как пусси";
                
            return _losePhrasesDictionary[_randomizer.Next(0, _losePhrasesDictionary.Count)];
        }
        private string GetScore() => $"со счётом {_participantStat.Kills}/{_participantStat.Deaths}/{_participantStat.Assists}";
        private string GetDamage()
        {
            double teamDamage = _matchInfo.Info.Participants
                .Where(p => p.TeamId == _participantStat.TeamId)
                .Sum(p => p.TotalDamageDealtToChampions);
            var damagePercentage = Math.Round(_participantStat.TotalDamageDealtToChampions / teamDamage * 100);

            var sb = new StringBuilder($"и нанёс {_participantStat.TotalDamageDealtToChampions} ({damagePercentage}%) урона!");
            return sb.ToString();
        }
        private async Task<string> GetRankedStat()
        {
            if (_matchInfo.Info.QueueId != 420) return string.Empty;
            
            var summonerInfo = _summonerRepository.GetAll()
                .First(s => s.Name == _summonerName);

            var currentLeague = (await _riotClient.GetLeagueInfo(summonerInfo.SummonerId)).FirstOrDefault(l =>
                l.QueueType == "RANKED_SOLO_5x5"); //TODO move to const

            if (currentLeague == null) return string.Empty;
                
            var sb = new StringBuilder();
            if (currentLeague.GetTierIntegerRepresentation() == summonerInfo.Tier && currentLeague.GetRankIntegerRepresentation() == summonerInfo.Rank)
            {
                if (_participantStat.Win) sb.Append("+");
                sb.Append($"{currentLeague.LeaguePoints - summonerInfo.LeaguePoints} LP. Текущий ранг {currentLeague.GetRankStringRepresentation()} {currentLeague.LeaguePoints} LP");
            }
            else
            {
                sb.Append(_participantStat.Win
                    ? $"Наш мальчик поднялся, теперь он {currentLeague.GetRankStringRepresentation()}!"
                    : $"Даунранкет до {currentLeague.GetRankStringRepresentation()} =(");
            }

            return sb.ToString();
        }
        
        private string GetPersonal()
        {
            if (!_participantStat.Win && _participantStat.SummonerName == "AidenGrimes") return "Но все равно молодец!";
            
            return string.Empty;
        }
    }
}