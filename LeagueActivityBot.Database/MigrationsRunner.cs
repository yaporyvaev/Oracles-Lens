using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LeagueActivityBot.Database
{
    public static class MigrationsRunner
    {
        public static async Task ApplyMigrations(ILogger logger, IServiceProvider serviceProvider, string appName)
        {
            var operationId = Guid.NewGuid().ToString().Substring(0, 4);
            logger.Log(LogLevel.Information,$"{appName}:UpdateDatabase:{operationId}: starting...");

            try
            {
                using (var serviceScope = serviceProvider.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                    await dbContext.Database.MigrateAsync();
                }

                logger.Log(LogLevel.Information,$"{appName}:UpdateDatabase:{operationId}: successfully done");
                await Task.FromResult(true);
            }
            catch (Exception exception)
            {
                logger.Log(LogLevel.Critical, exception, $"{appName}:UpdateDatabase.{operationId}: Migration failed");
                throw;
            }
        }
    }
}
