using Microsoft.AspNetCore.Mvc;

namespace LeagueActivityBot.Host
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}