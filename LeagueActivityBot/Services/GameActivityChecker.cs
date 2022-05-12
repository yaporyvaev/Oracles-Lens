using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Models;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Notifications.OnSoloGameStarted;

namespace LeagueActivityBot.Services
{
    public class GameActivityChecker
    {
        private readonly IRiotClient _riotClient;
        private readonly GameInfoInMemoryRepository _gameInfoRepository;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<GameActivityChecker> _logger;

        public GameActivityChecker(
            IRiotClient riotClient,
            GameInfoInMemoryRepository gameInfoRepository,
            IRepository<Summoner> summonerRepository,
            IMediator mediator,
            ILogger<GameActivityChecker> logger)
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

            var tasks = new List<Task>(summoners.Count);
            foreach (var summoner in summoners)
            {
                tasks.Add(ProcessSummoner(summoner, gameParticipantsHelper));
            }

            await Task.WhenAll(tasks);
        }

        private async Task ProcessSummoner(Summoner summoner, GameParticipantsHelper gameParticipantsHelper)
        {
            var currentGameInfo = await _riotClient.GetCurrentGameInfo(summoner.SummonerId);
            using var summonerCurrentGameScope = _logger.BeginScope(new Dictionary<string, string> {
                    { "SummonerName", summoner.Name},
                    { "GameId", currentGameInfo .GameId.ToString() },
                    { "GameInfo", JsonConvert.SerializeObject(currentGameInfo ) },
                });

            if (currentGameInfo.IsInGameNow && _gameInfoRepository.GameExists(summoner.Name))
            {
                var cachedGame = _gameInfoRepository.GetGame(summoner.Name);
                if (cachedGame.GameId != currentGameInfo.GameId)
                {
                    _gameInfoRepository.RemoveGame(summoner.Name);
                    if (gameParticipantsHelper.IsSoloGame(cachedGame.Participants))
                    {
                        await _mediator.Publish(new OnSoloGameEndedNotification(summoner, cachedGame.GameId));
                    }
                    _logger.LogError("Игра удалена из скип-блока");
                }
                else
                {
                    return;
                }
            }

            if (!currentGameInfo.IsInGameNow && _gameInfoRepository.GameExists(summoner.Name))
            {
                await RemoveActivity(summoner, gameParticipantsHelper);
                return;
            }

            if (currentGameInfo.IsInGameNow && !_gameInfoRepository.GameExists(summoner.Name))
            {
                await AddActivity(summoner, currentGameInfo, gameParticipantsHelper);
            }
        }

        private async Task RemoveActivity(Summoner summoner, GameParticipantsHelper gameParticipantsHelper)
        {
            try
            {
                var lastGameInfo = _gameInfoRepository.GetGame(summoner.Name);

                if (gameParticipantsHelper.IsSoloGame(lastGameInfo.Participants))
                {
                    await _mediator.Publish(new OnSoloGameEndedNotification(summoner, lastGameInfo.GameId));
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при удалении записи и уведомлении.");
            }

            _gameInfoRepository.RemoveGame(summoner.Name);
        }

        private async Task AddActivity(Summoner summoner, CurrentGameInfo currentGameInfo, GameParticipantsHelper gameParticipantsHelper)
        {
            if (_gameInfoRepository.GetLastGameId(summoner.Name) == currentGameInfo.GameId) return;

            await UpdateLeagueInfo(summoner);

            if (gameParticipantsHelper.IsSoloGame(currentGameInfo.Participants))
            {
                await _mediator.Publish(new OnSoloGameStartedNotification(summoner, currentGameInfo.GameId, currentGameInfo.GameQueueConfigId));
            }

            _gameInfoRepository.AddGame(summoner.Name, currentGameInfo);
        }
        
        private async Task UpdateLeagueInfo(Summoner summoner)
        {
            var leagueInfo = (await _riotClient.GetLeagueInfo(summoner.SummonerId))
                .FirstOrDefault(l => l.QueueType == QueueType.RankedSolo);

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