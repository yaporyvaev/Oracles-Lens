using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
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
        private readonly SummonersInMemoryRepository _summonersRepository;
        private readonly GameInfoInMemoryRepository _gameInfoRepository;
        private readonly GameParticipantsHelper _gameParticipantsHelper;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GameActivityCheckerJob(
            IRiotClient riotClient, 
            SummonersInMemoryRepository summonersRepository, 
            GameInfoInMemoryRepository gameInfoRepository, 
            GameParticipantsHelper gameParticipantsHelper, IServiceScopeFactory serviceScopeFactory)
        {
            _riotClient = riotClient;
            _summonersRepository = summonersRepository;
            _gameInfoRepository = gameInfoRepository;
            _gameParticipantsHelper = gameParticipantsHelper;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            foreach (var summoner in _summonersRepository.SummonersInfo)
            {
                var gameInfo = await _riotClient.GetCurrentGameInfo(summoner.Id);
                
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
                    _gameInfoRepository.AddGame(summoner.Name, gameInfo);
                    
                    if (_gameParticipantsHelper.IsSoloGameForSummoner(summoner.Name, gameInfo))
                    {
                        await mediator.Publish(new OnSoloGameStartedNotification(summoner.Name, gameInfo.GameId));
                    }
                }
            }
        }
    }
}