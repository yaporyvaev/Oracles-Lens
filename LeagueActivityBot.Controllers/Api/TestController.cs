using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using LeagueActivityBot.Contracts;
using LeagueActivityBot.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Controllers.Api
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly SummonersInMemoryRepository _summonersRepository;
        private readonly IRiotClient _riotClient;

        public TestController(SummonersInMemoryRepository summonersRepository, IRiotClient riotClient)
        {
            _summonersRepository = summonersRepository;
            _riotClient = riotClient;
        }
        
        [HttpGet("summ-init")]
        public IActionResult InitializeSummonersStore()
        {
            var summoners = _summonersRepository.SummonersInfo;
            return Ok(summoners);
        }
        
        [HttpGet("activity")]
        public async Task<IActionResult> CheckActivity()
        {
            var games = new List<UserInGameInfo>();
            foreach (var summoner in _summonersRepository.SummonersInfo)
            {
                var gameInfo = await _riotClient.GetCurrentGameInfo(summoner.Id);
                if (gameInfo.IsInGameNow)
                {
                    games.Add(new UserInGameInfo
                    {
                        SummonerName = summoner.Name,
                        GameId = gameInfo.GameId,
                        GameMode = gameInfo.GameMode,
                        GameStartTime = DateTimeOffset.FromUnixTimeMilliseconds(gameInfo.GameStartTime).LocalDateTime,
                        ParticipantNames = gameInfo.Participants.Select(p => p.SummonerName).ToArray()
                    });
                }
            }

            return Ok(games);
        }
        
        [HttpGet("match")]
        public async Task<IActionResult> Match([FromQuery] long gameId)
        {
            var matchInfo = await _riotClient.GetMatchInfo(gameId);
            if (matchInfo == null) return NotFound();

            var matchResult = new MatchResult
            {
                Scores = new List<Score>()
            };
            
            foreach (var participant in matchInfo.Info.Participants)
            {
                if(_summonersRepository.FindByName(participant.SummonerName) == null) continue;
                
                matchResult.Scores.Add(new Score
                {
                    SummonerName = participant.SummonerName,
                    ChampionName = participant.ChampionName,
                    Kills = participant.Kills,
                    Deaths = participant.Deaths,
                    Assists = participant.Assists,
                    Win = participant.Win
                });
            }
            
            return Ok(matchResult);
        }
    }
}