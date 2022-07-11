using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Notifications.OnGameStarted;
using LeagueActivityBot.Notifications.OnSoloGameStarted;
using LeagueActivityBot.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LeagueActivityBot.Services
{
    public class StartGameChecker
    {
        private readonly IRiotClient _riotClient;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<StartGameChecker> _logger;

        public StartGameChecker(
            IRiotClient riotClient,
            IRepository<GameInfo> gameInfoRepository,
            IRepository<Summoner> summonerRepository,
            IMediator mediator,
            ILogger<StartGameChecker> logger)
        {
            _riotClient = riotClient;
            _gameInfoRepository = gameInfoRepository;
            _summonerRepository = summonerRepository;
            _mediator = mediator;
            _logger = logger;
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
                        IsProcessed = false,
                        QueueId = currentGameInfo.GameQueueConfigId,
                        GameStartTime = DateTimeOffset.FromUnixTimeMilliseconds(currentGameInfo.GameStartTime)
                            .LocalDateTime,
                        SummonerNamesJson = JsonConvert.SerializeObject(gameParticipants.Select(p => p.Name))
                    };

                    await _gameInfoRepository.Add(gameInfo);

                    if (gameParticipants.Length == 1)
                    {
                        await UpdateLeagueInfo(summoner); //TODO update flex league too
                        await _mediator.Publish(new OnSoloGameStartedNotification(summoner, currentGameInfo.GameId,
                            currentGameInfo.GameQueueConfigId));
                    }
                    else
                    {
                        await _mediator.Publish(new OnGameStartedNotification(gameParticipants, currentGameInfo.GameId,
                            currentGameInfo.GameQueueConfigId));
                    }

                    summonerNamesToSkip.AddRange(gameParticipants.Select(s => s.Name));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Start game check failed for {summoner.Name}");
                }
            }
        }

        private async Task UpdateLeagueInfo(Summoner summoner)
        {
            var leagueInfo = (await _riotClient.GetLeagueInfo(summoner.SummonerId))
                .FirstOrDefault(l => l.QueueType == QueueTypeConstants.RankedSolo);

            if (leagueInfo != null)
            {
                summoner.LeaguePoints = leagueInfo.LeaguePoints;
                summoner.Tier = leagueInfo.GetTierIntegerRepresentation();
                summoner.Rank = leagueInfo.GetRankIntegerRepresentation();
                await _summonerRepository.Update(summoner);
            }
        }
    }
}