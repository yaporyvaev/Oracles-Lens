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

                var jobKey = new JobKey("GameActivityCheckerJob");
                q.AddJob<GameActivityCheckerJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("GameActivityCheckerJob-trigger")
                    .WithCronSchedule("0 * * ? * *")); // run every 1 minute
            });
            
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            
            return services;
        }
    }
}