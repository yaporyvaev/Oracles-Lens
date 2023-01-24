using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Services;

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
                .ToList();
            
            var allMatchIds = await GetAllMathIds(summoners.Select(s => s.Puuid).ToHashSet());
            await UpdateMatchInfo(allMatchIds, summoners);
        }

        private async Task UpdateMatchInfo(IEnumerable<string> matchIds, IEnumerable<Summoner> summoners)
        {
            foreach (var matchId in matchIds)
            {
                var matchIdNumValue = long.Parse(matchId.Split("_").Last());
                var gameInfo = _gameInfoRepository.GetAll(true)
                    .FirstOrDefault(g => g.GameId == matchIdNumValue);
                if (gameInfo != null) continue;
                
                var matchInfo = await _riotClient.GetMatchInfo(matchId);
                if(matchInfo == null) continue;
                
                await _gameService.UpdateGameInfo(matchInfo, summoners.ToDictionary(s => s.Puuid, x => x));
                await Task.Delay(3000); //Rate limit
            }
        }
        
        private async Task<HashSet<string>> GetAllMathIds(IEnumerable<string> summonerIds)
        {
            var allMatchIds = new HashSet<string>();
            
            foreach (var summonerId in summonerIds)
            {
                var skip = 0;
                const int take = 100;

                List<string> matchIds;
                do
                {
                    matchIds = await _riotClient.GetMatchIds(summonerId, skip, take);
                    foreach (var matchId in matchIds)
                    {
                        if (!allMatchIds.Contains(matchId))
                        {
                            allMatchIds.Add(matchId);
                        }
                    }

                    skip += take;
                    await Task.Delay(3000); //Rate limit
                } while (matchIds.Any());
            }
            
            return allMatchIds;
        }
    }
}