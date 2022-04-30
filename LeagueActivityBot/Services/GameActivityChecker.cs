using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constatnts;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Helpers;
using LeagueActivityBot.Notifications;
using LeagueActivityBot.Repository;
using MediatR;

namespace LeagueActivityBot.Services
{
    public class GameActivityChecker
    {
        private readonly IRiotClient _riotClient;
        private readonly GameInfoInMemoryRepository _gameInfoRepository;
        private readonly IRepository<Summoner> _summonerRepository;
        private readonly IMediator _mediator;

        public GameActivityChecker(
            IRiotClient riotClient,
            GameInfoInMemoryRepository gameInfoRepository,
            IRepository<Summoner> summonerRepository, IMediator mediator)
        {
            _riotClient = riotClient;
            _gameInfoRepository = gameInfoRepository;
            _summonerRepository = summonerRepository;
            _mediator = mediator;
        }

        public async Task Check()
        {
            var summoners = _summonerRepository.GetAll().ToList();
            var gameParticipantsHelper = new GameParticipantsHelper(summoners.Select(s => s.Name));

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

            //Если в игре и запись существует - скип
            if (currentGameInfo.IsInGameNow && _gameInfoRepository.GameExists(summoner.Name)) return;

            //Если не в игре и запись существует - удаляем запись
            if (!currentGameInfo.IsInGameNow && _gameInfoRepository.GameExists(summoner.Name))
            {
                try
                {
                    var lastGameInfo = _gameInfoRepository.GetGame(summoner.Name);

                    if (gameParticipantsHelper.IsSoloGame(lastGameInfo.Participants))
                    {
                        await _mediator.Publish(new OnSoloGameEndedNotification(summoner, lastGameInfo.GameId));
                    }
                }
                catch (Exception)
                {
                    //TODO log
                }

                _gameInfoRepository.RemoveGame(summoner.Name);
                return;
            }

            //Если в игре и записи не существует - добавляем запись
            if (currentGameInfo.IsInGameNow && !_gameInfoRepository.GameExists(summoner.Name))
            {
                //Если эта игра уже была обработана - скип
                if(_gameInfoRepository.GetLastGameId(summoner.Name) == currentGameInfo.GameId)
                    return;
                    
                await UpdateLeagueInfo(summoner);

                if (gameParticipantsHelper.IsSoloGame(currentGameInfo.Participants))
                {
                    await _mediator.Publish(new OnSoloGameStartedNotification(summoner.Name, currentGameInfo.GameId, currentGameInfo.GameQueueConfigId ));
                }

                _gameInfoRepository.AddGame(summoner.Name, currentGameInfo);
            }
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