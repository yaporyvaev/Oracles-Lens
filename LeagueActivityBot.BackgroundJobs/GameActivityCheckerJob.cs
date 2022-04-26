using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Notifications;
using LeagueActivityBot.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class GameActivityCheckerJob : IJob
    {
        private readonly IRiotClient _riotClient;
        private readonly GameInfoInMemoryRepository _gameInfoRepository;
        private readonly GameParticipantsHelper _gameParticipantsHelper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRepository<Summoner> _summonerRepository;

        public GameActivityCheckerJob(
            IRiotClient riotClient, 
            GameInfoInMemoryRepository gameInfoRepository, 
            GameParticipantsHelper gameParticipantsHelper, IServiceScopeFactory serviceScopeFactory, IRepository<Summoner> summonerRepository)
        {
            _riotClient = riotClient;
            _gameInfoRepository = gameInfoRepository;
            _gameParticipantsHelper = gameParticipantsHelper;
            _serviceScopeFactory = serviceScopeFactory;
            _summonerRepository = summonerRepository;
        }

        //TODO Refactor this shit
        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var summoners = _summonerRepository.GetAll()
                .ToList();
            
            foreach (var summoner in summoners)
            {
                var gameInfo = await _riotClient.GetCurrentGameInfo(summoner.SummonerId);
                
                //Если в игре и запись существует - скип
                if (gameInfo.IsInGameNow && _gameInfoRepository.GameExists(summoner.Name))
                {
                    continue;
                }
                
                //Если не в игре и запись существует - удаляем с уведомлением
                if (!gameInfo.IsInGameNow && _gameInfoRepository.GameExists(summoner.Name))
                {
                    var lastGameInfo = _gameInfoRepository.GetLastGame(summoner.Name);

                    if (_gameParticipantsHelper.IsSoloGameForSummoner(summoner.Name, lastGameInfo))
                    {
                        await mediator.Publish(new OnSoloGameEndedNotification(summoner.Name, lastGameInfo.GameId));
                    }

                    _gameInfoRepository.RemoveGame(summoner.Name);
                    continue;
                }

                //Если в игре и записи не существует - добавляем с уведомлением 
                if (gameInfo.IsInGameNow && !_gameInfoRepository.GameExists(summoner.Name))
                {
                    var leagueInfo = (await _riotClient.GetLeagueInfo(summoner.SummonerId))
                        .FirstOrDefault(l => l.QueueType == "RANKED_SOLO_5x5");
                    
                    if (leagueInfo != null)
                    {
                        summoner.LeaguePoints = leagueInfo.LeaguePoints;
                        summoner.Tier = leagueInfo.GetTierIntegerRepresentation();
                        summoner.Rank = leagueInfo.GetTierIntegerRepresentation();
                        await _summonerRepository.Update(summoner);
                    }
                    
                    
                    if (_gameParticipantsHelper.IsSoloGameForSummoner(summoner.Name, gameInfo))
                    {
                        await mediator.Publish(new OnSoloGameStartedNotification(summoner.Name, gameInfo.GameId));
                    }
                    
                    _gameInfoRepository.AddGame(summoner.Name, gameInfo);
                }
            }
        }
    }
}