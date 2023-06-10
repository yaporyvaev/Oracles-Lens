using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LeagueActivityBot.Host.Infrastructure
{
    [UsedImplicitly]
    public class ApiKeyMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(
            IConfiguration configuration,
            RequestDelegate next)
        {
            _configuration = configuration;
            _next = next;
        }

        private string ApiKey => _configuration["App:ApiKey"];
        
        [UsedImplicitly]
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments(new PathString("/api")))
            {
                await _next.Invoke(context);
                return;
            }

            var headerApiKey = context.Request.Headers["x-api-key"].FirstOrDefault();
            if (ApiKey == null || headerApiKey != ApiKey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new {message = "Invalid API Key"}));

                return;
            }

            await _next.Invoke(context);
        }
    }
}
