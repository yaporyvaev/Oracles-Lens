using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Notifications.OnGameStarted;
using LeagueActivityBot.Notifications.OnSoloGameStarted;
using LeagueActivityBot.Utils;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LeagueActivityBot.Services
{
    public class StartGameChecker
    {
        private readonly IRiotClient _riotClient;
        private readonly LeagueService _leagueService;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<StartGameChecker> _logger;

        public StartGameChecker(
            IRiotClient riotClient,
            IRepository<GameInfo> gameInfoRepository,
            IRepository<Summoner> summonerRepository,
            IMediator mediator,
            ILogger<StartGameChecker> logger, LeagueService leagueService)
        {
            _riotClient = riotClient;
            _gameInfoRepository = gameInfoRepository;
            _summonerRepository = summonerRepository;
            _mediator = mediator;
            _logger = logger;
            _leagueService = leagueService;
        }

        public async Task Check()
        {
            var summoners = _summonerRepository.GetAll().ToList();
            var gameParticipantsHelper = new GameParticipantsHelper(summoners);
            var summonerNamesToSkip = new HashSet<string>();

            foreach (var summoner in summoners)
            {
                try
                {
                    if (summonerNamesToSkip.Contains(summoner.Name)) continue;

                    var currentGameInfo = await _riotClient.GetCurrentGameInfo(summoner.SummonerId);
                    if (!currentGameInfo.IsInGameNow) continue;

                    var existingGame = _gameInfoRepository.GetAll()
                        .FirstOrDefault(g => g.GameId == currentGameInfo.GameId);
                    if (existingGame != null) continue;

                    var gameParticipants = gameParticipantsHelper.GetSummonersInGame(currentGameInfo.Participants)
                        .ToArray();

                    var gameInfo = new GameInfo
                    {
                        GameId = currentGameInfo.GameId,
                        QueueId = currentGameInfo.GameQueueConfigId,
                    };
                    gameInfo.GameParticipants = gameParticipants.Select(s => new GameParticipant {SummonerId = s.Id, GameInfo = gameInfo}).ToList();

                    await _gameInfoRepository.Add(gameInfo);

                    if (gameParticipants.Length == 1)
                    {
                        await _mediator.Publish(new OnSoloGameStartedNotification(summoner, currentGameInfo.GameId, currentGameInfo.GameQueueConfigId));
                    }
                    else
                    {
                        await _mediator.Publish(new OnTeamGameStartedNotification(gameParticipants, currentGameInfo.GameId, currentGameInfo.GameQueueConfigId));
                    }

                    if (gameInfo.IsRankedGame)
                    {
                        await _leagueService.UpdateLeague(gameParticipants);
                    }
                    
                    summonerNamesToSkip.AddRange(gameParticipants.Select(s => s.Name));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Start game check failed for {summoner.Name}");
                }
            }
        }
    }
}