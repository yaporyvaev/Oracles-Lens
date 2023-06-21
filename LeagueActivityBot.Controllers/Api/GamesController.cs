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
        private readonly ChampionInfoService _championInfoService;
        private readonly IMapper _mapper;
        
        public GamesController(GameService gameService, SummonerService summonerService, IMapper mapper, ChampionInfoService championInfoService)
        {
            _gameService = gameService;
            _summonerService = summonerService;
            _mapper = mapper;
            _championInfoService = championInfoService;
        }

        [HttpGet("{gameId}/info")]
        public async Task<IActionResult> GetGames(long gameId)
        {
            var gameInfo = await _gameService.GetGameInfo(gameId);
            if (gameInfo == null) return NotFound();
            var summoners = await _summonerService.GetSummoners();

            var gameInfoResponseDto = _mapper.Map<GameInfoDto>(gameInfo.Info);

            #region Fill summoner's icon field
            foreach (var participant in gameInfoResponseDto.Participants)
            {
                participant.ChampionIconUrl = await _championInfoService.GetChampionIconUrl(participant.ChampionId);
            }
            #endregion
            
            var response = new GetGameInfoResponse
            {
                GameInfo = gameInfoResponseDto,
                RegisteredSummoners = _mapper.Map<SummonerDto[]>(summoners),
            };
            
            return Ok(response);
        }
        
    }
}