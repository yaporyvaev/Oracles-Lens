using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace LeagueActivityBot.BackgroundJobs
{
    public static class Entry
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddQuartz(q =>  
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var gameActivityCheckerJobKey = new JobKey("GameActivityCheckerJob");
                q.AddJob<GameActivityCheckerJob>(opts => opts.WithIdentity(gameActivityCheckerJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(gameActivityCheckerJobKey)
                    .WithIdentity("GameActivityCheckerJob-trigger")
                    .WithCronSchedule("0 * * ? * *")); // run every 1 minute
            });
            
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            
            return services;
        }
    }
}