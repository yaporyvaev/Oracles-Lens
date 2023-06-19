using System.Linq;
using System.Threading.Tasks;
using LeagueActivityBot.Contracts.Summoners;
using LeagueActivityBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummonersController : ControllerBase
    {
        private readonly SummonerService _summonerService;

        public SummonersController(SummonerService summonerService)
        {
            _summonerService = summonerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSummoners()
        {
            var summoners = await _summonerService.GetSummonersWithLeague();

            var result = new GetSummonersResponse {Summoners = summoners.Select(s => s.Name).ToArray()};
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddSummoner([FromBody] AddSummonerRequest request)
        {
            await _summonerService.AddSummoner(request.SummonerName);
            return Ok();
        }
        
        [HttpDelete]
        public async Task<IActionResult> RemoveSummoner([FromBody] RemoveSummonerRequest request)
        {
            await _summonerService.RemoveSummoner(request.SummonerName);
            return Ok();
        }
    }
}