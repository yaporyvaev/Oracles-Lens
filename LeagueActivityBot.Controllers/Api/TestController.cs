using System.Threading.Tasks;
using LeagueActivityBot.Telegram.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Controllers.Api
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ImageService _imageService;

        public TestController(ImageService  imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _imageService.SendImage();
            return Ok();
        }
    }
}