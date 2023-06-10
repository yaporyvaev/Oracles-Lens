using System.Threading.Tasks;
using LeagueActivityBot.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Controllers.Api
{
    [Route("api/[controller]")]
    public class ClashController : ControllerBase
    {
        private readonly IRiotClient _riotClient;

        public ClashController(IRiotClient riotClient)
        {
            _riotClient = riotClient;
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            var clashInfos = await _riotClient.GetClashSchedule();
            return Ok(clashInfos);
        }
    }
}