using System.Threading.Tasks;
using LeagueActivityBot.Contracts.Score;
using LeagueActivityBot.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeightsController : ControllerBase
    {
        private readonly WeightsService _weightsService;

        public WeightsController(WeightsService weightsService)
        {
            _weightsService = weightsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddWeights([FromBody] AddWeightsRequest request)
        {
            await _weightsService.AddScore(request);
            return Ok();
        }
    }
}