using System.Threading.Tasks;
using AutoMapper;
using LeagueActivityBot.Contracts.Game;
using LeagueActivityBot.Contracts.Summoners;
using LeagueActivityBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Controllers.Api
{
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly SummonerService _summonerService;
        private readonly IMapper _mapper;
        
        public GamesController(GameService gameService, SummonerService summonerService, IMapper mapper)
        {
            _gameService = gameService;
            _summonerService = summonerService;
            _mapper = mapper;
        }

        [HttpGet("{gameId}/info")]
        public async Task<IActionResult> GetGames(long gameId)
        {
            var gameInfo = await _gameService.GetGameInfo(gameId);
            if (gameInfo == null) return NotFound();
            var summoners = await _summonerService.GetSummoners();

            var response = new GetGameInfoResponse
            {
                GameInfo = _mapper.Map<GameInfoDto>(gameInfo.Info),
                RegisteredSummoners = _mapper.Map<SummonerDto[]>(summoners)
            };
            
            return Ok(response);
        }
    }
}