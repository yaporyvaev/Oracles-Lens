using System.Reflection;
using LeagueActivityBot.Controllers.Api;
using Microsoft.Extensions.DependencyInjection;

namespace LeagueActivityBot.Controllers
{
    public static class Entry
    {
        public static IMvcBuilder AddApi(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(Assembly.GetAssembly(typeof(TestController)));
        }
    }
}