using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Constants;
using LeagueActivityBot.Entities;
using LeagueActivityBot.Models;
using LeagueActivityBot.Notifications.OnSoloGameEnded;
using LeagueActivityBot.Notifications.OnTeamGameEnded;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LeagueActivityBot.Services
{
    public class GameService
    {
        private readonly IRepository<GameInfo> _gameInfoRepository;
        private readonly IRepository<GameParticipant> _gameParticipantRepository;
        private readonly IRiotClient _riotClient;
        private readonly IMediator _mediator;
        private readonly LeagueService _leagueService;        
        private readonly IMemoryCache _memoryCache;


        public GameService(
            IRepository<GameInfo> gameInfoRepository, 
            IRiotClient riotClient, 
            IMediator mediator, 
            IRepository<GameParticipant> gameParticipantRepository, 
            LeagueService leagueService, 
            IMemoryCache memoryCache)
        {
            _gameInfoRepository = gameInfoRepository;
            _riotClient = riotClient;
            _mediator = mediator;
            _gameParticipantRepository = gameParticipantRepository;
            _leagueService = leagueService;
            _memoryCache = memoryCache;
        }
        
        public async Task<MatchInfo> GetGameInfo(long gameId)
        {
            var gameCacheKey = $"game_info_{gameId}";
            var matchInfo = _memoryCache.Get<MatchInfo>(gameCacheKey);

            if (matchInfo == null)
            {
                matchInfo = await _riotClient.GetMatchInfo(gameId);
                
                if(matchInfo != null)
                    _memoryCache.Set(gameCacheKey, matchInfo, TelegramMessageOptions.MessageTimeToLive.Add(TimeSpan.FromMinutes(30)));
            }
            
            return matchInfo;
        }
        
        public async Task ProcessEndGame(GameInfo game)
        {
            var summoners = game.GameParticipants.Select(p => p.Summoner).ToArray();
            var gameInfo = await GetGameInfo(game.GameId);
            if (gameInfo == null) return;

            Dictionary<string, EndGameLeagueDelta> leagueDelta = null;
            if (game.IsRankedGame)
            {
                leagueDelta = await _leagueService.GetDeltaAndUpdateLeague(summoners, game.QueueId);
            }
            
            if (summoners.Length > 1)
            {
                await _mediator.Publish(new OnTeamGameEndedNotification(summoners, gameInfo, leagueDelta));
            }
            else
            {
                await _mediator.Publish(new OnSoloGameEndedNotification(summoners.First(), gameInfo, leagueDelta?.FirstOrDefault().Value));
            }
            
            //TODO refactor mapping
            foreach (var participant in game.GameParticipants)
            {
                var matchParticipant = gameInfo.Info.Participants.First(p => p.SummonerId == participant.Summoner.SummonerId);
                participant.Assists = matchParticipant.Assists;
                participant.Deaths = matchParticipant.Deaths;
                participant.Kills = matchParticipant.Kills;
                participant.Win = matchParticipant.Win;
                participant.ChampionId = matchParticipant.ChampionId;
                participant.ChampionName = matchParticipant.ChampionName;
                participant.CreepScore = matchParticipant.TotalMinionsKilled;
                participant.PentaKills = matchParticipant.PentaKills;
                participant.VisionScore = matchParticipant.VisionScore;
                participant.DetectorWardsPlaced = matchParticipant.DetectorWardsPlaced;
                participant.FirstBloodKill = matchParticipant.FirstBloodKill;

                await _gameParticipantRepository.Update(participant);
            }

            game.IsProcessed = true;
            game.GameStartTime = gameInfo.Info.GameStartTime;
            game.GameDurationInSeconds = gameInfo.Info.GameDurationInSeconds;
            game.GameEnded = true;
            
            await _gameInfoRepository.Update(game);
        }
        
        /// <summary>
        /// Try to update gameInfo
        /// </summary>
        /// <param name="matchInfo">Match info from Riot API</param>
        /// <param name="summoners">Summoners list in DB</param>
        public async Task UpdateGameInfo(MatchInfo matchInfo, IDictionary<string,Summoner> summoners)
        {
            var gameInfo = _gameInfoRepository.GetAll(true)
                .FirstOrDefault(g => g.GameId == matchInfo.Info.GameId);
            if (gameInfo != null)
            {
                await _gameInfoRepository.HardRemove(gameInfo);
            }
            
            //TODO refactor mapping
            gameInfo = new GameInfo
            {
                GameEnded = true,
                GameId = matchInfo.Info.GameId,
                IsDeleted = false,
                IsProcessed = true,
                QueueId = matchInfo.Info.QueueId,
                GameStartTime = matchInfo.Info.GameStartTime,
                GameDurationInSeconds = matchInfo.Info.GameDurationInSeconds,
                GameParticipants = new List<GameParticipant>()
            };

            foreach (var matchParticipant in matchInfo.Info.Participants)
            {
                if(!summoners.ContainsKey(matchParticipant.Puuid)) continue;
                var participant = new GameParticipant
                {
                    Assists = matchParticipant.Assists,
                    Deaths = matchParticipant.Deaths,
                    Kills = matchParticipant.Kills,
                    Win = matchParticipant.Win,
                    ChampionId = matchParticipant.ChampionId,
                    ChampionName = matchParticipant.ChampionName,
                    CreepScore = matchParticipant.TotalMinionsKilled,
                    PentaKills = matchParticipant.PentaKills,
                    VisionScore = matchParticipant.VisionScore,
                    DetectorWardsPlaced = matchParticipant.DetectorWardsPlaced,
                    FirstBloodKill = matchParticipant.FirstBloodKill,
                    
                    SummonerId = summoners[matchParticipant.Puuid].Id
                };
                
                gameInfo.GameParticipants.Add(participant);
            }
            
            await _gameInfoRepository.Add(gameInfo);
        }
    }
}