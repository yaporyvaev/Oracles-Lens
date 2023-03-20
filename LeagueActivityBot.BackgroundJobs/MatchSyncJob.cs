using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Services;
using Microsoft.EntityFrameworkCore;

namespace LeagueActivityBot.BackgroundJobs
{
    public class MatchSyncJob 
    {
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IRiotClient _riotClient;
        private readonly GameService _gameService;


        public MatchSyncJob(IRepository<Summoner> summonerRepository, IRiotClient riotClient, GameService gameService, IRepository<GameInfo> gameInfoRepository)
        {
            _summonerRepository = summonerRepository;
            _riotClient = riotClient;
            _gameService = gameService;
            _gameInfoRepository = gameInfoRepository;
        }

        public async Task Sync()
        {
            var summoners = _summonerRepository
                .GetAll()
                .ToArray();
            
            var matchIdsMap = await GetAllMathIdsMap(summoners.Select(s => s.Puuid));
            await UpdateMatchInfo(matchIdsMap, summoners);
        }

        private async Task UpdateMatchInfo(Dictionary<string,int> matchIdsMap, Summoner[] summoners)
        {
            var summonersPuuids = summoners.Select(s => s.Puuid).ToArray();
            foreach (var matchMap in matchIdsMap)
            {
                var matchIdNumValue = long.Parse(matchMap.Key.Split("_").Last());
                var localDbMatchParticipantsPuuids = _gameInfoRepository.GetAll(true)
                    .Include(g => g.GameParticipants)
                    .ThenInclude(p => p.Summoner)
                    .Where(g => g.GameId == matchIdNumValue)
                    .Select(g => g.GameParticipants.Select(p => p.Summoner.Puuid))
                    .FirstOrDefault();

                if (localDbMatchParticipantsPuuids != null)
                {
                    var summonersInGameCount = localDbMatchParticipantsPuuids.Intersect(summonersPuuids).Count();
                    if(matchMap.Value == summonersInGameCount) continue;
                }
                
                var matchInfo = await _riotClient.GetMatchInfo(matchMap.Key);
                if(matchInfo == null) continue;
                
                await _gameService.UpdateGameInfo(matchInfo, summoners.ToDictionary(s => s.Puuid, x => x));
                await Task.Delay(3000); //Rate limit
            }
        }
        
        private async Task<Dictionary<string,int>> GetAllMathIdsMap(IEnumerable<string> summonerPuuids)
        {
            var allMatchIdsMap = new Dictionary<string,int>();
            
            foreach (var summonerId in summonerPuuids)
            {
                var skip = 0;
                const int take = 100;

                List<string> matchIds;
                do
                {
                    matchIds = await _riotClient.GetMatchIds(summonerId, skip, take);
                    foreach (var matchId in matchIds)
                    {
                        if (!allMatchIdsMap.ContainsKey(matchId))
                        {
                            allMatchIdsMap.Add(matchId, 1);
                        }
                        else
                        {
                            allMatchIdsMap[matchId]++;
                        }
                    }

                    skip += take;
                    await Task.Delay(3000); //Rate limit
                } while (matchIds.Any());
            }
            
            return allMatchIdsMap;
        }
    }
}